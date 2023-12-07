using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressBar : MonoBehaviour
{
    public Image progressBar;

    public Canvas WinScreen;

    public Canvas LoseScreen;

    public SetSpawner setSpawner;

    public int maxObjectsToDodge = 10;

    private int dodgedObjects = 0;
    private int hitObjects = 0;

    public float timeToComplete = 60f;
    public float currentTime = 0f;
    private bool won = false;
    private bool lost = false;

    void Start()
    {
        timeToComplete = setSpawner.LevelLengthInSec;
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        maxObjectsToDodge = setSpawner.AmountOfSets;
        UpdateProgress();
    }

    private void Update()
    {
        timeToComplete = setSpawner.LevelLengthInSec;
        maxObjectsToDodge = setSpawner.AmountOfSets;
        if (!won)
            currentTime += Time.deltaTime;
    }

    public void DodgeObject()
    {
        dodgedObjects++;

        UpdateProgress();

        if ((dodgedObjects - hitObjects) >= maxObjectsToDodge && !lost)
        {
            setSpawner._spawning = false;
            won = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                Destroy(touchObjects[i].gameObject);
            }

            WinScreen.gameObject.SetActive(true);
        }

        if (currentTime > timeToComplete)
        {
            setSpawner._spawning = false;
            lost = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                Destroy(touchObjects[i].gameObject);
            }
            LoseScreen.gameObject.SetActive(true);
        }
    }

    public void HitObject()
    {
        hitObjects++;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        Debug.Log(currentTime.ToString());
        if (!won && !lost)
        {
            float progress = (float)(dodgedObjects - hitObjects) / maxObjectsToDodge;
            progressBar.fillAmount = progress;
        }
    }
}
