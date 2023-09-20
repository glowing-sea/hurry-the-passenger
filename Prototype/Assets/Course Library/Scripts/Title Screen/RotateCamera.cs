using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script rotate the camera about the plane on the title screen
public class RotateCamera : MonoBehaviour
{

    public float rotationSpeed; // rotation speed of the camera

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
