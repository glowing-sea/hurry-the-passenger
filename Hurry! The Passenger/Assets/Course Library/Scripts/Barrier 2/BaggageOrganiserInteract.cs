using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script make the baggage organiser to be interactive
// When play interact with it, it go to the baggage organisation view
// It also check if the play has sucessfully organised items in thier 3 bags
public class BaggageOrganiserInteract : MonoBehaviour
{
    // Interactable object variable
    public TextMeshProUGUI interact;
    private bool interactable;


    public GameObject baggageOrganiserMenu;
    public GameObject[] items; // a list of item to be organnise

    public TextMeshProUGUI largeText; // print text on the UI screen

    // Script
    private GameManager gameManager; // reference to the game manager script


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); // get reference
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // if the player has not finished organising their baggage
                if (gameManager.gameState == GameState.Running && !gameManager.tasks[1])
                {
                    gameManager.gameState = GameState.LeavingMainScene;
                    // go into baggage organisation view
                    gameManager.playerCamera.enabled = false; // !!! Test
                    gameManager.baggageCamera.enabled = true;

                    interact.gameObject.SetActive(false);
                    gameManager.staminaGauge.gameObject.SetActive(false);
                    baggageOrganiserMenu.SetActive(true);
                }
            }
        }
    }

    // Show [F] when the player close to it
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not finished organising their baggage
        if (other.gameObject.CompareTag("Player") && !gameManager.tasks[1])
        {
            interact.gameObject.SetActive(true);
            interactable = true;
        }
    }

    // Hide [F] when the player leaves it
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interact.gameObject.SetActive(false);
            interactable = false;
        }
    }

    // When the player want to exit the baggage organisation view
    public void ExitButton()
    {
        baggageOrganiserMenu.SetActive(false);
        interact.gameObject.SetActive(true);
        gameManager.gameState = GameState.Running;
        gameManager.playerCamera.enabled = true;
        gameManager.baggageCamera.enabled = false;
    }

    // When the play want to confirm their baggage organisation
    public void ConfirmButton()
    {
        // Organisation Complete
        if (isBaggageWellOrganise())
        {
            ExitButton();

            largeText.text = "Organisation Complete!";
            StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
            gameManager.soundEffect.PlayOneShot(gameManager.taskComplete, 1.0f);
            gameManager.tasks[1] = true;
            gameManager.UpdateNotesMenu();
        }
        // Organisation Complete
        else
        {
            largeText.text = "Organisation Incomplete!";
            StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
            gameManager.soundEffect.PlayOneShot(gameManager.somethingWrong, 1.0f);
        }

    }

    // Check if all items are well organisation in player's baggage
    public bool isBaggageWellOrganise()
    {
        int[] totalWeight = new int[4];
        Draggable script;

        for (int i = 0; i < items.Length; i++)
        {
            script =  items[i].GetComponent<Draggable>();

            // Item not in any bag or overlaping with other bags
            if (script.isOverLap || script.inWhichBag == 0)
            {
                return false;
            }
            else
            {
                totalWeight[script.inWhichBag] += script.weight;
            }
        }

        // Check if all bag is under 20kg
        for (int i = 1; i < totalWeight.Length; i++)
        {
            if (totalWeight[i] > 20)
            {
                return false;
            }
        }
        return true;
    }
}
