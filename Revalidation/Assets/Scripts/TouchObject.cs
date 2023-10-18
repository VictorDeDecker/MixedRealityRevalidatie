using UnityEngine;

public class TouchObject : MonoBehaviour
{
    public string TargetTag = "Target";
    public float Speed = 1f;
    private bool _touched = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_touched)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
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
