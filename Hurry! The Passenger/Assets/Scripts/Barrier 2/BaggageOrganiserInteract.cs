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
    public Camera baggageCamera; // the camera for baggage organisation view

    // Interactable object variable
    private TextMeshProUGUI interact;
    private bool interactable;


    public GameObject baggageOrganiserMenu;
    public GameObject[] items; // a list of item to be organnise

    [SerializeField] PlayerTask baggageTask;

    private TextMeshProUGUI largeText; // print text on the UI screen

    // Script
    private GameManager gameManager; // reference to the game manager script


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
        largeText = gameManager.mainUI.largeArbitraryText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.F) && GameManager.instance.gameState == GameState.Running && !gameManager.GetTaskState(baggageTask).isComplete)
        {
            baggageOrganiserMenu.SetActive(true); // open special menu
            interact.gameObject.SetActive(false); // close interact prompt
            gameManager.gameState = GameState.LeavingMainScene; // go into special game state that player cannot move
            baggageCamera.depth = 1; // bring security camera forward
            gameManager.staminaGauge.gameObject.SetActive(false); // hide stamina if player is running
            gameManager.mainUI.minimap.SetActive(false); // hide minimap
            gameManager.mainUI.taskIcon.SetActive(false);
            gameManager.mainUI.guideIcon.SetActive(false);
            gameManager.mainUI.pauseIcon.SetActive(false);
        } 
    }

    // Show [F] when the player close to it
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not finished organising their baggage
        if (other.gameObject.CompareTag("Player") && !gameManager.GetTaskState(baggageTask).isComplete)
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
        baggageOrganiserMenu.SetActive(false); // close special menu
        interact.gameObject.SetActive(true); // reopen interact prompt
        gameManager.gameState = GameState.Running; // reset game state
        baggageCamera.depth = -1; // bring security camera back
        gameManager.mainUI.minimap.SetActive(true); // reopen minimap
        gameManager.mainUI.taskIcon.SetActive(true);
        gameManager.mainUI.guideIcon.SetActive(true);
        gameManager.mainUI.pauseIcon.SetActive(true);
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
            gameManager.CompleteTask(baggageTask);
        }
        // Organisation Complete
        else
        {
            largeText.text = "Organisation Incomplete!";
            StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
            gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
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
