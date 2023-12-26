using UnityEngine;

public class CloseTaskMenu : MonoBehaviour
{
    private float currentTime = 0f;
    public Canvas TaskCanvas;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 3)
        {
            TaskCanvas.gameObject.SetActive(false);
        }
    }
}