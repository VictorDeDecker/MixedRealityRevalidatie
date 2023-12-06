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

    public float timeToComplete = 60f;
    public float currentTime = 0f;
    private bool won = false;
    private bool lost = false;

    void Start()
    {
        timeToComplete = setSpawner.LevelLengthInSec;
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        maxObjectsToDodge = Mathf.FloorToInt(setSpawner.AmountOfSets * (setSpawner.SetWidth * setSpawner.MaxPercentageOfMissingObjects));
        UpdateProgress();
    }

    private void Update()
    {
        if (!won)
            currentTime += ((int)Time.deltaTime);
    }

    public void DodgeObject()
    {
        maxObjectsToDodge = Mathf.FloorToInt(setSpawner.AmountOfSets * (setSpawner.SetWidth * setSpawner.MaxPercentageOfMissingObjects));
        dodgedObjects++;

        UpdateProgress();

        if (dodgedObjects >= maxObjectsToDodge && !lost)
        {
            won = true;
            WinScreen.gameObject.SetActive(true);
        }

        if (currentTime > timeToComplete)
        {
            lost = true;
            LoseScreen.gameObject.SetActive(true);
        }
    }

    public void HitObject()
    {
        dodgedObjects--;
    }

    private void UpdateProgress()
    {
        if (!won && !lost)
        {
            maxObjectsToDodge = Mathf.FloorToInt(setSpawner.AmountOfSets * (setSpawner.SetWidth * setSpawner.MaxPercentageOfMissingObjects));
            Debug.Log(maxObjectsToDodge + ", dodged: " + dodgedObjects);
            float progress = (float)dodgedObjects / maxObjectsToDodge;
            progressBar.fillAmount = progress;
        }
    }
}
