using UnityEngine;
using UnityEngine.InputSystem;

public class XrHandler : MonoBehaviour
{
    public Canvas canvas;
    private InputAction primaryButtonAction;

    private void Awake()
    {
        InitializeInput();
    }

    private void OnEnable()
    {
        if (primaryButtonAction != null)
        {
            primaryButtonAction.Enable();
        }
        else
        {
            Debug.Log("primaryButtonAction is null");
        }
    }

    private void OnDisable()
    {
        if (primaryButtonAction != null)
        {
            primaryButtonAction.Disable();
        }
    }

    private void InitializeInput()
    {
        // Set up the input action
        primaryButtonAction = new InputAction("OpenMenu", binding: "<XRController>{RightHand}/{PrimaryButton}");
        primaryButtonAction.performed += OnPrimaryButtonPress;
    }

    private void OnPrimaryButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canvas.gameObject.activeSelf)
                canvas.gameObject.SetActive(false);
            else
                canvas.gameObject.SetActive(true);
        }
    }
}
