using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressBar : MonoBehaviour
{
    public Image progressBar;
    public Canvas WinScreen;
    public Canvas LoseScreen;
    public TextMeshProUGUI RedFishScore;
    public TextMeshProUGUI PinkFishScore;
    public TextMeshProUGUI GreenFishScore;
    public TextMeshProUGUI YellowFishScore;
    public UnityServer UnityServer;

    public float TimeToComplete = 60f;
    public float CurrentTime = 0f;

    //The amount of fishes a pati�nt has to hit
    public float AmountOfRedFishesToHit = 0f;
    public float AmountOfPinkFishesToHit = 0f;
    public float AmountOfGreenFishesToHit = 0f;
    public float AmountOfYellowFishesToHit = 0f;

    //The actual amount the pati�nt has hit
    private float RedFishesHit = 0f;
    private float PinkFishesHit = 0f;
    private float GreenFishesHit = 0f;
    private float YellowFishesHit = 0f;

    //The amount of fishes missed
    private float RedFishesMissed = 0f;
    private float PinkFishesMissed = 0f;
    private float GreenFishesMissed = 0f;
    private float YellowFishesMissed = 0f;

    //All fishes caught per color
    public bool AllRedFishesCaught = false;
    public bool AllPinkFishesCaught = false;
    public bool AllGreenFishesCaught = false;
    public bool AllYellowFishescaught = false;

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

    void Awake()
    {
        if (UnityServer == null)
        {
            UnityServer = FindObjectOfType<UnityServer>();
        }
        AmountOfRedFishesToHit = UnityServer.RedFish;
        AmountOfPinkFishesToHit = UnityServer.PinkFish;
        AmountOfGreenFishesToHit = UnityServer.GreenFish;
        AmountOfYellowFishesToHit = UnityServer.YellowFish;
    }

    public void UpdateAmountOfFish()
    {
        if (UnityServer == null)
        {
            UnityServer = FindObjectOfType<UnityServer>();
        }
        AmountOfRedFishesToHit = UnityServer.RedFish;
        AmountOfPinkFishesToHit = UnityServer.PinkFish;
        AmountOfGreenFishesToHit = UnityServer.GreenFish;
        AmountOfYellowFishesToHit = UnityServer.YellowFish;
        TimeToComplete = UnityServer.objectSpawner.LevelLengthInSec;
    }

    void Start()
    {
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        SetFishScores();
        amountOfObjectsToHit = AmountOfRedFishesToHit + AmountOfPinkFishesToHit + AmountOfGreenFishesToHit + AmountOfYellowFishesToHit;
        UpdateProgress();
    }

    private void Update()
    {
        if (!won)
            CurrentTime += Time.deltaTime;
        CheckIfLost();
    }

    public void MissedObject(bool IsTargetFish, string color)
    {
        if (IsTargetFish)
        {
            missedObjects++;
            switch (color.ToLower())
            {
                case "red":
                    RedFishesMissed++;
                    break;
                case "pink":
                    PinkFishesMissed++;
                    break;
                case "green":
                    GreenFishesMissed++;
                    break;
                case "yellow":
                    YellowFishesMissed++;
                    break;
            }
        }

        SetFishScores();
        CheckColorAmount();
        UpdateProgress();
    }

    public void HitNotTargetFish()
    {
        hitNotTargetFish++;

        AddRandomMissedFish();

        SetFishScores();
        CheckColorAmount();
        UpdateProgress();
    }

    public void HitObstacle()
    {
        hitObstacle++;
        AddRandomMissedFish();

        SetFishScores();
        CheckColorAmount();
        UpdateProgress();
    }

    public void HitObjectWithHead()
    {
        hitObjectWithHead++;
        AddRandomMissedFish();

        SetFishScores();
        CheckColorAmount();
        UpdateProgress();
    }

    public void HitObject(string hand, string color)
    {
        hitObjects++;

        UpdateColorHit(color);
        UpdateHandHit(hand);

        if (hitObjects - (missedObjects + hitObjectWithHead + hitObstacle + (hitNotTargetFish / 2)) > amountOfObjectsToHit && !lost && CheckIfHasWon())
        {
            UnityServer.objectSpawner.IsSpawning = false;
            won = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                Destroy(touchObjects[i].gameObject);
            }

            WinScreen.gameObject.SetActive(true);
        }
        UpdateProgress();
    }

    private void CheckIfLost()
    {
        if (CurrentTime > TimeToComplete)
        {
            UnityServer.objectSpawner.IsSpawning = false;
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
            case "red":
                RedFishesHit++;
                break;
            case "pink":
                PinkFishesHit++;
                break;
            case "green":
                GreenFishesHit++;
                break;
            case "yellow":
                YellowFishesHit++;
                break;
        }

        SetFishScores();
        CheckColorAmount();
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
        if (AllRedFishesCaught &&
            AllPinkFishesCaught &&
            AllGreenFishesCaught &&
            AllYellowFishescaught)
            return true;

        return false;
    }

    private void SetFishScores()
    {
        if (AmountOfRedFishesToHit == 0)
            RedFishScore.text = "";
        else
            RedFishScore.text = $"{RedFishesHit}/{AmountOfRedFishesToHit} Red Fish ({RedFishesMissed} missed)";

        if (AmountOfPinkFishesToHit == 0)
            PinkFishScore.text = "";
        else
            PinkFishScore.text = $"{PinkFishesHit}/{AmountOfPinkFishesToHit} Pink Fish ({PinkFishesMissed} missed)";

        if (AmountOfGreenFishesToHit == 0)
            GreenFishScore.text = "";
        else
            GreenFishScore.text = $"{GreenFishesHit}/{AmountOfGreenFishesToHit} Green Fish ({GreenFishesMissed} missed)";

        if (AmountOfYellowFishesToHit == 0)
            YellowFishScore.text = "";
        else
            YellowFishScore.text = $"{YellowFishesHit}/{AmountOfYellowFishesToHit} Yellow Fish ({YellowFishesMissed} missed)";
    }

    public void CheckColorAmount()
    {
        if (RedFishesHit - RedFishesMissed >= AmountOfRedFishesToHit)
        {
            AllRedFishesCaught = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                if (touchObjects[i].Color.ToLower() == "red")
                    Destroy(touchObjects[i].gameObject);
            }
        }
        else
        {
            AllRedFishesCaught = false;
        }

        if (PinkFishesHit - PinkFishesMissed >= AmountOfPinkFishesToHit)
        {
            AllPinkFishesCaught = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                if (touchObjects[i].Color.ToLower() == "pink")
                    Destroy(touchObjects[i].gameObject);
            }
        }
        else
        {
            AllPinkFishesCaught = false;
        }

        if (GreenFishesHit - GreenFishesMissed >= AmountOfGreenFishesToHit)
        {
            AllGreenFishesCaught = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                if (touchObjects[i].Color.ToLower() == "green")
                    Destroy(touchObjects[i].gameObject);
            }
        }
        else
        {
            AllGreenFishesCaught = false;
        }

        if (YellowFishesHit - YellowFishesMissed >= AmountOfYellowFishesToHit)
        {
            AllYellowFishescaught = true;
            var touchObjects = FindObjectsOfType<TouchObject>();

            for (int i = 0; i < touchObjects.Length; i++)
            {
                if (touchObjects[i].Color.ToLower() == "yellow")
                    Destroy(touchObjects[i].gameObject);
            }
        }
        else
        {
            AllYellowFishescaught = false;
        }
    }

    private void AddRandomMissedFish()
    {
        bool foundPossibleFish = false;
        while (foundPossibleFish)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    if (AmountOfRedFishesToHit != 0)
                    {
                        foundPossibleFish = true;
                        RedFishesMissed++;
                    }

                    break;
                case 1:
                    if (AmountOfPinkFishesToHit != 0)
                    {
                        foundPossibleFish = true;
                        PinkFishesMissed++;
                    }
                    break;
                case 2:
                    if (AmountOfGreenFishesToHit != 0)
                    {
                        foundPossibleFish = true;
                        GreenFishesMissed++;
                    }
                    break;
                case 3:
                    if (AmountOfYellowFishesToHit != 0)
                    {
                        foundPossibleFish = true;
                        YellowFishesMissed++;
                    }
                    break;
            }
        }
    }
}