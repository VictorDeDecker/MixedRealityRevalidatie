using UnityEngine;

public class VRHeadsetCollision : MonoBehaviour
{
    public UpdateProgressBar progressBar;
    // Start is called before the first frame update
    void Start()
    {
        if (progressBar == null)
            progressBar = FindObjectOfType<UpdateProgressBar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Volumes") && !other.CompareTag("FishNet"))
        {
            progressBar.HitObjectWithHead();
            Destroy(other.gameObject);
        }
    }
}