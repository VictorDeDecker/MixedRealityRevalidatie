using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressBar : MonoBehaviour
{
    public Image progressBar;
    public Canvas WinScreen;
    public Canvas LoseScreen;
    public ObjectSpawnerV2 ObjectSpawnerV2;

    public float TimeToComplete = 60f;
    public float CurrentTime = 0f;

    //The amount of fishes a patiënt has to hit
    public float AmountOfRedFishesToHit = 0f;
    public float AmountOfPinkFishesToHit = 0f;
    public float AmountOfGreenFishesToHit = 0f;
    public float AmountOfYellowFishesToHit = 0f;

    //The actual amount the patiënt has hit
    private float RedFishesHit = 0f;
    private float PinkFishesHit = 0f;
    private float GreenFishesHit = 0f;
    private float YellowFishesHit = 0f;

    private float amountOfObjectsToHit = 10;
    private float missedObjects = 0;
    private float hitObjects = 0;
    private float hitObjectWithHead = 0;
    private float hitObstacle = 0;
    private float hitNotTargetFish = 0;
    private bool won = false;
    private bool lost = false;
    private float hitWithRightHand = 0;
    private float hitWithLeftHand = 0;

    void Start()
    {
        TimeToComplete = ObjectSpawnerV2.LevelLengthInSec;
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        amountOfObjectsToHit = AmountOfRedFishesToHit + AmountOfPinkFishesToHit + AmountOfGreenFishesToHit + AmountOfYellowFishesToHit;
        UpdateProgress();
    }

    private void Update()
    {
        TimeToComplete = ObjectSpawnerV2.LevelLengthInSec;
        if (!won)
            CurrentTime += Time.deltaTime;
    }

    public void MissedObject(bool IsTargetFish)
    {
        if (IsTargetFish)
        {
            missedObjects++;
        }


        UpdateProgress();
    }

    public void HitNotTargetFish()
    {
        hitNotTargetFish++;
        UpdateProgress();
    }

    public void HitObstacle()
    {
        hitObstacle++;
        UpdateProgress();
    }

    public void HitObjectWithHead()
    {
        hitObjectWithHead++;
        UpdateProgress();
    }

    public void HitObject(string hand, string color)
    {
        hitObjects++;
        UpdateColorHit(color);

        UpdateHandHit(hand);

        if (hitObjects > amountOfObjectsToHit && !lost && CheckIfHasWon())
        {
            ObjectSpawnerV2.IsSpawning = false;
            won = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                Destroy(touchObjects[i].gameObject);
            }

            WinScreen.gameObject.SetActive(true);
        }

        if (CurrentTime > TimeToComplete)
        {
            ObjectSpawnerV2.IsSpawning = false;
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

    private void UpdateColorHit(string color)
    {
        switch (color.ToLower())
        {
            case "Red":
                RedFishesHit++;
                break;
            case "Pink":
                PinkFishesHit++;
                break;
            case "Green":
                GreenFishesHit++;
                break;
            case "Yellow":
                YellowFishesHit++;
                break;
        }
    }

    private void UpdateHandHit(string hand)
    {
        if (hand.ToLower() == "right")
            hitWithRightHand++;
        else if (hand.ToLower() == "left")
            hitWithLeftHand++;
    }

    private void UpdateProgress()
    {
        if (!won && !lost)
        {
            var progress = (hitObjects - (missedObjects + hitObjectWithHead + hitObstacle + (hitNotTargetFish / 2))) / amountOfObjectsToHit;
            progressBar.fillAmount = progress;
        }
    }

    private bool CheckIfHasWon()
    {
        if (RedFishesHit >= AmountOfRedFishesToHit &&
            PinkFishesHit >= AmountOfPinkFishesToHit &&
            GreenFishesHit >= AmountOfGreenFishesToHit &&
            YellowFishesHit >= AmountOfYellowFishesToHit)
            return true;

        return false;
    }
}