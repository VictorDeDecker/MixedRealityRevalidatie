using UnityEngine;

public class TouchObject : MonoBehaviour
{
    public GameObject Target;
    public float Speed;
    private bool _touched;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_touched)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Target)
        {
            Debug.Log($"{Target} touched {this}");
            _touched = true;
        }
    }
}
