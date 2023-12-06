using UnityEngine;

public class ClosePauseMenu : MonoBehaviour
{
    public Canvas canvas;
    public void CloseMenu()
    {
        if (canvas.gameObject.activeSelf)
            canvas.gameObject.SetActive(false);
        else
            canvas.gameObject.SetActive(true);
    }
}
