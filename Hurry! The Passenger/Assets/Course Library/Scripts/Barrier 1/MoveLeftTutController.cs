using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftTutController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            // delete the object
            Destroy(gameObject);
        }
    }
}
