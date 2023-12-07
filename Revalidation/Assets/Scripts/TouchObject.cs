using UnityEngine;

public class TouchObject : MonoBehaviour
{
    public string TargetTag = "Target";
    public float Speed = 1f;
    private bool _touched = false;
    public float Rotation = 0f;
    public Vector3 RockDirection = new Vector3(0f, 0f, 1f);
    public Vector3 ShipDirection = new Vector3(0f, 0f, 1f);
    public Vector3 BalloonDirection = new Vector3(0f, 0f, -1f);
    public Vector3 SeagullDirection = new Vector3(0f, 0f, -1f);
    public UpdateProgressBar ProgressBar;

    void Start()
    {
        if (ProgressBar == null)
        {
            ProgressBar = FindObjectOfType<UpdateProgressBar>();
        }
        transform.Rotate(0, Rotation, 0);

        if (transform.CompareTag("Ship"))
        {
            transform.Rotate(new Vector3(90f, 0, 0));
        }

        if (transform.CompareTag("Balloon"))
        {
            transform.Rotate(new Vector3(-90f, 0, 0));
        }

        if (transform.CompareTag("Seagull"))
        {
            transform.Rotate(new Vector3(0, 180f, 0));
        }
    }

    void Update()
    {
        if (_touched)
        {
            Destroy(gameObject);
        }

        if (transform.CompareTag("RockTarget"))
        {
            transform.Translate(Speed * Time.deltaTime * RockDirection, Space.World);
        }
        else if (transform.CompareTag("Ship"))
        {
            transform.Translate(Speed * Time.deltaTime * ShipDirection, Space.World);
        }
        else if (transform.CompareTag("Balloon"))
        {
            transform.Translate(Speed * Time.deltaTime * BalloonDirection, Space.World);
        }
        else if (transform.CompareTag("Seagull"))
        {
            transform.Translate(Speed * Time.deltaTime * SeagullDirection, Space.World);
        }
        else if (transform.CompareTag("Submarine"))
        {
            transform.Translate(Speed * Time.deltaTime * Vector3.forward, Space.World);
        }
        else
        {
            transform.Translate(Speed * Time.deltaTime * Vector3.left);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TargetTag))
        {
            ProgressBar.DodgeObject();
            _touched = true;
        }
    }
}
