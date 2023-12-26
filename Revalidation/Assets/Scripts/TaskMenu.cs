using TMPro;
using UnityEngine;

public class TaskMenu : MonoBehaviour
{
    public UpdateProgressBar ProgressBar;
    public TextMeshProUGUI RedFish;
    public TextMeshProUGUI PinkFish;
    public TextMeshProUGUI GreenFish;
    public TextMeshProUGUI YellowFish;
    void Start()
    {
        if (ProgressBar == null)
            ProgressBar = FindObjectOfType<UpdateProgressBar>();

        if (ProgressBar.AmountOfRedFishesToHit != 0)
        {
            RedFish.gameObject.SetActive(true);
            RedFish.text = $"Catch {ProgressBar.AmountOfRedFishesToHit} red fish";
        }
        else
            RedFish.gameObject.SetActive(false);

        if (ProgressBar.AmountOfPinkFishesToHit != 0)
        {
            PinkFish.gameObject.SetActive(true);
            PinkFish.text = $"Catch {ProgressBar.AmountOfPinkFishesToHit} pink fish";
        }
        else
            PinkFish.gameObject.SetActive(false);

        if (ProgressBar.AmountOfGreenFishesToHit != 0)
        {
            GreenFish.gameObject.SetActive(true);
            GreenFish.text = $"Catch {ProgressBar.AmountOfGreenFishesToHit} green fish";
        }
        else
            GreenFish.gameObject.SetActive(false);

        if (ProgressBar.AmountOfYellowFishesToHit != 0)
        {
            YellowFish.gameObject.SetActive(true);
            YellowFish.text = $"Catch {ProgressBar.AmountOfYellowFishesToHit} yellow fish";
        }
        else
            YellowFish.gameObject.SetActive(false);

    }

    void Update()
    {

    }
}