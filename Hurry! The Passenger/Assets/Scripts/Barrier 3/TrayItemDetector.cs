using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayItemDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            other.GetComponent<ItemAttributes>().onTheTray = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            other.GetComponent<ItemAttributes>().onTheTray = false;
        }
    }
}
