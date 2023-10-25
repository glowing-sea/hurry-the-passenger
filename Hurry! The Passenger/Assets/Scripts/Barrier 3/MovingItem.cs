using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : MonoBehaviour
{
    private SecurityCheckMinigame minigame;
    private bool isOnConveyor = false;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        minigame = GameObject.Find("Security Check (Minigame)").GetComponent<SecurityCheckMinigame>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOnConveyor)
            rb.velocity = new Vector3(-minigame.conveyorSpeed, 0, 0);
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
