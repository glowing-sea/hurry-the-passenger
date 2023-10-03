using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardBackwards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         this.gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
           if (Input.GetKeyDown("a"))
        {
             this.gameObject.GetComponent<Renderer>().enabled = true;
        }

           if (Input.GetKeyDown("w"))
        {
            // delete the object
            Destroy(gameObject);
        }
    }
}
