using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTray : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody rb;
    private SecurityCheckMinigame minigame;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        minigame = GameObject.Find("Security Check (Minigame)").GetComponent<SecurityCheckMinigame>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(-minigame.conveyorSpeed, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DeleteTrigger")){
            Destroy(this);
        }
    }
}
