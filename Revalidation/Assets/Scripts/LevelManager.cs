using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
