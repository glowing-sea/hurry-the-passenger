using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutCrateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //if collides with player, delete self
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    }


}
