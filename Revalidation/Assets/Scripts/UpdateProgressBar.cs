using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressBar : MonoBehaviour
{
    public Image progressBar;

    public Canvas WinScreen;

    public Canvas LoseScreen;

    public ObjectSpawnerV2 objectSpawnerV2;
    public float amountOfObjectsToHit = 10;

    private float missedObjects = 0;
    private float hitObjects = 0;

    public float timeToComplete = 60f;
    public float currentTime = 0f;
    private bool won = false;
    private bool lost = false;

    void Start()
    {
        timeToComplete = objectSpawnerV2.LevelLengthInSec;
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        UpdateProgress();
    }

    private void Update()
    {
        timeToComplete = objectSpawnerV2.LevelLengthInSec;
        if (!won)
            currentTime += Time.deltaTime;
    }

    public void DodgeObject()
    {
        missedObjects++;

        UpdateProgress();
    }

    public void HitObjectWithHead()
    {

    }

    public void HitObject()
    {
        hitObjects++;

        if (hitObjects > amountOfObjectsToHit && !lost)
        {
            objectSpawnerV2.IsSpawning = false;
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
            objectSpawnerV2.IsSpawning = false;
            lost = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                Destroy(touchObjects[i].gameObject);
            }
            LoseScreen.gameObject.SetActive(true);
        }
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        if (!won && !lost)
        {
            var progress = hitObjects / amountOfObjectsToHit;
            progressBar.fillAmount = progress;
        }
    }
}
