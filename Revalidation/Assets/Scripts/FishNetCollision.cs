using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FishNetCollision : MonoBehaviour
{
    public UpdateProgressBar progressBar;
    public string Hand = "Right";
    private XRGrabInteractable grabInteractable;
    void Start()
    {
        if (progressBar == null)
            progressBar = FindObjectOfType<UpdateProgressBar>();

        grabInteractable = gameObject.GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component not found on the fishnet GameObject.");
        }
        else
        {
            grabInteractable.onSelectEntered.AddListener((value) => { grabInteractable.interactionLayerMask = LayerMask.GetMask("Nothing"); });
        }
    }

    public void SetHand(string hand)
    {
        Hand = hand;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Volumes")) return;

        if (other.CompareTag("Fish"))
        {
            if (other.gameObject.GetComponent<TouchObject>() is not TouchObject touchObject) return;

            if (touchObject.IsTargetFish)
            {
                progressBar.HitObject(Hand, touchObject.Color);
                Destroy(other.gameObject);
            }
            else
            {
                progressBar.HitNotTargetFish();
            }

        }
        else if (other.CompareTag("Ship") || other.CompareTag("RockTarget") || other.CompareTag("Submarine") || other.CompareTag("Shark"))
        {
            progressBar.HitObstacle();
            Destroy(other.gameObject);
        }
    }
}
