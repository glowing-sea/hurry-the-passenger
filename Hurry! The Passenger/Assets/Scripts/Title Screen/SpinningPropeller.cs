using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script spin the propeller of the plane on the title screen
public class SpinningPropeller : MonoBehaviour
{
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Spin the propeller
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime * 20);
    }
}
