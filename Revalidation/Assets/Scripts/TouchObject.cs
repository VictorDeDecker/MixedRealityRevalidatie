using UnityEngine;

public class TouchObject : MonoBehaviour
{
    public string TargetTag = "Target";
    public float Speed = 1f;
    private bool _touched = false;
    public float Rotation = 0f;
    public Vector3 RockDirection = new Vector3(0f, 0f, 1f);
    public Vector3 ShipDirection = new Vector3(0f, 0f, 1f);

    void Start()
    {
        transform.Rotate(0, Rotation, 0);

        if (transform.tag == "Ship")
        {
            transform.Rotate(new Vector3(90f, 0, 0));
        }
    }

    void Update()
    {
        if (_touched)
        {
            Destroy(gameObject);
        }

        if (transform.tag == "RockTarget")
        {
            transform.Translate(RockDirection * Speed * Time.deltaTime, Space.World);
        }
        else if (transform.tag == "Ship")
        {
            transform.Translate(ShipDirection * Speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TargetTag)
        {
            Debug.Log($"{other} touched {this}");
            _touched = true;
        }
    }
}
