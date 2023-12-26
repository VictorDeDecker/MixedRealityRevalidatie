using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public string sceneToLoad;

    public void LoadLevel(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    public string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}