using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.z >= -35f && transform.rotation.y <= 30)
        {
                this.transform.Rotate(new Vector3(0,0.5f));
                this.transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z + 0.02f);
        }
        else if(transform.rotation.y > 30)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.02f);
        }
        else
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.02f);
        }
        
    }
}
