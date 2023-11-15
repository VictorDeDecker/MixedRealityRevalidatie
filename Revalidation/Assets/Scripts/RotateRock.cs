using UnityEngine;

public class RotateRock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(35f * Time.deltaTime, 55f * Time.deltaTime, 40f * Time.deltaTime, Space.Self);
    }
}
