using UnityEngine;

public class FishNetCollision : MonoBehaviour
{
    public UpdateProgressBar progressBar;
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
            progressBar.HitObject();
            Destroy(other.gameObject);
        }
    }
}
