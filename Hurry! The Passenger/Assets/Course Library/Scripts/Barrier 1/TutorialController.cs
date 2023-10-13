using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

      //if collides with player, set game state to running.
    private void OnTriggerEnter(Collider other){
        Debug.Log("yaaaaaaaaaaaaaaaaaaaaay " + other.gameObject.name);
        if (other.gameObject.name == "Player"  && gameManager.gameState != GameState.Over)
        {
            gameManager.gameState = GameState.Running;
            //print("collided");
            Debug.Log("collided");
        }
    }


}


