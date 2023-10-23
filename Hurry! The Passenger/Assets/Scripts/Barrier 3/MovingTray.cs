using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTray : MonoBehaviour
{
    private static float speed = 4f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(-speed, 0, 0);
        // rb.AddForce(new Vector3(-speed, 0, 0), ForceMode.VelocityChange);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Scan Finish Trigger")
            Destroy(gameObject);
    }

}
