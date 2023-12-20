using UnityEngine;

public class FishNetCollision : MonoBehaviour
{
    public UpdateProgressBar progressBar;
    public string Hand = "Right";
    void Start()
    {
        if (progressBar == null)
            progressBar = FindObjectOfType<UpdateProgressBar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Volumes")) return;

        if (other.CompareTag("Fish"))
        {
            if (other.gameObject.GetComponent<TouchObject>() is not TouchObject touchObject) return;

            if (touchObject.IsTargetFish)
                progressBar.HitObject(Hand, touchObject.Color);
            else
                progressBar.HitNotTargetFish();

            Destroy(other.gameObject);

        }
        else if (other.CompareTag("Ship") || other.CompareTag("RockTarget") || other.CompareTag("Submarine") || other.CompareTag("Shark"))
        {
            progressBar.HitObstacle();
            Destroy(other.gameObject);
        }
    }
}