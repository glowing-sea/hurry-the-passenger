using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrinkDealerInteract : MonoBehaviour
{


    private bool interactable;

    private bool alreadyDrank = false; // prevent player from drinking more than once


    // Interactable object variable
    TextMeshProUGUI interact;


    // Script
    private GameManager gameManager; // reference to the game manager script

    public PlayerController playerController; // reference to the player controller script
   
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        playerController = GameObject.FindObjectOfType<PlayerController>();
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player is close to this object and interactable
        if (interactable && !alreadyDrank)
        {
            // If the player press F
            if (Input.GetKeyDown(KeyCode.F))
            {
             
                playerController.DrinkEnergyDrink(); //grants energy drink fueled super run
                //decrease money
                gameManager.UpdateBalance(100,false);
                alreadyDrank = true;
                

            }
        }
    }

    // Show interact key [f] in the UI
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not checked in
        if (other.gameObject.CompareTag("Player"))
        {
            interact.gameObject.SetActive(true);
            interactable = true;
        }
    }

    // Exit the interaction
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interact.gameObject.SetActive(false);
            interactable = false;
        }
    }
}
