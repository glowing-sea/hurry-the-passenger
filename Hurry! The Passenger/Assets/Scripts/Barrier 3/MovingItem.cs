using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : MonoBehaviour
{
    private SecurityCheckInteract securityCheckInteract;
    private bool isOnConveyor = false;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        securityCheckInteract = GameObject.Find("Security Check Interact").GetComponent<SecurityCheckInteract>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOnConveyor)
            rb.velocity = new Vector3(-securityCheckInteract.conveyorSpeed, 0, 0);
        //rb.AddForce(new Vector3(-securityCheckInteract.conveyorSpeed, 0, 0), ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Conveyor")
        {
            isOnConveyor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Conveyor")
        {
            isOnConveyor = false;
        }
    }
}
