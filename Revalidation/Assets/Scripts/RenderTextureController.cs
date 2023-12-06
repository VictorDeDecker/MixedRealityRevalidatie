using UnityEngine;

public class RenderTextureController : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Texture2D capturedTexture;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        RenderTexture.active = renderTexture;
        capturedTexture = new(renderTexture.width, renderTexture.height);
        capturedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        capturedTexture.Apply();
        RenderTexture.active = null;
    }
}
