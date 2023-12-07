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

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        progressBar.HitObject();
        Destroy(other.gameObject);
    }
}
