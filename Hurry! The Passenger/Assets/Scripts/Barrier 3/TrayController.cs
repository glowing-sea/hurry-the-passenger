using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayController : MonoBehaviour
{
    [System.NonSerialized] public float speed = 4f;
    private Rigidbody rb;
    private SecurityCheckMinigame minigame;
    public GameObject itemDetactor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        minigame = SecurityCheckMinigame.instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(-minigame.conveyorSpeed, 0, 0);
    }
}
