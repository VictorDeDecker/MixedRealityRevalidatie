using UnityEngine;
using UnityEngine.InputSystem;

public class SensitivityIncrease : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Debug.Log(Touchscreen.current.device);

            //Vector3 movement = new Vector3(input.x, 0, input.y) * speed * Time.deltaTime;

            //transform.Translate(movement);
        }
    }
}