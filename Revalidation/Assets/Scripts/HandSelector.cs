using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandSelector : MonoBehaviour
{
    public XRGrabInteractable Righthand;
    public XRGrabInteractable LeftHand;
    public XRRayInteractor RightHandInteractor;
    public XRRayInteractor LeftHandInteractor;
    public XRInteractionManager InteractionManager;
    void Start()
    {
        RightHandInteractor.keepSelectedTargetValid = true;
        LeftHandInteractor.keepSelectedTargetValid = true;
        if (Righthand == null || LeftHand == null)
        {
            var xRGrabInteractable = FindObjectsOfType<XRGrabInteractable>();

            foreach (var interactable in xRGrabInteractable)
            {
                if (interactable.name == "FishNetRightHand")
                    Righthand = interactable;
                else if (interactable.name == "FishNetLeftHand")
                    LeftHand = interactable;
            }
        }

        if (RightHandInteractor == null || LeftHandInteractor == null)
        {
            var xRRayInteractor = FindObjectsOfType<XRRayInteractor>();

            foreach (var interactor in xRRayInteractor)
            {
                if (interactor.name == "RightHand Controller")
                    RightHandInteractor = interactor;
                else if (interactor.name == "LeftHand Controller")
                    LeftHandInteractor = interactor;
            }
        }

        if (InteractionManager == null)
        {
            InteractionManager = FindObjectOfType<XRInteractionManager>();
        }
    }
    public void SelectHand(string hand)
    {
        try
        {
            IXRSelectInteractable selectRight = Righthand;
            IXRSelectInteractable selectLeft = LeftHand;
            switch (hand)
            {
                case "both":
                    Righthand.gameObject.SetActive(true);
                    InteractionManager.SelectEnter(RightHandInteractor, selectRight);
                    LeftHand.gameObject.SetActive(true);
                    InteractionManager.SelectEnter(LeftHandInteractor, selectLeft);
                    break;
                case "left":
                    LeftHand.gameObject.SetActive(true);
                    InteractionManager.SelectEnter(LeftHandInteractor, selectLeft);
                    Righthand.gameObject.SetActive(false);
                    break;
                case "right":
                    LeftHand.gameObject.SetActive(false);
                    Righthand.gameObject.SetActive(true);
                    InteractionManager.SelectEnter(RightHandInteractor, selectRight);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
