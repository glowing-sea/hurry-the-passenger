using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterInteract : MonoBehaviour
{
    private bool interactable;

    public bool rightCounter;

    // Interactable object variable
    TextMeshProUGUI interact;
    TextMeshProUGUI largeText;
    TextMeshProUGUI smallText;


    // Script
    private GameManager gameManager; // reference to the game manager script
    public BaggageOrganiserInteract baggageOrganiser; // reference to BaggageOrganiserInteract script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
        largeText = gameManager.mainUI.largeArbitraryText.GetComponent<TextMeshProUGUI>();
        smallText = gameManager.mainUI.smallArbitraryText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // If have not checked in
        if (!gameManager.tasks[2])
        {
            // If the player nearby
            if (interactable)
            {
                // If the player press F
                if (Input.GetKeyDown(KeyCode.F))
                {
                     // Player goes to the right counter and has organized baggage
                    if (rightCounter && baggageOrganiser.isBaggageWellOrganise())
                    {
                        largeText.text = "Check-in Completed!\n Please go to the International Departure!\nTime is Running Out!";
                        StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
                        interactable = false;
                        interact.gameObject.SetActive(false);
                        gameManager.tasks[2] = true;
                        gameManager.UpdateNotesMenu();
                        gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
                    }
                    // Player goes to the right counter but hasn't organized baggage
                    else if (rightCounter)
                    {
                        smallText.text = "Please organise your baggage first!\nBaggae Organiser is on the right side of the Airport Entrance";
                        StartCoroutine(gameManager.ShowThingTemporarily(smallText.gameObject, 2));
                        gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
                    }
                    // Player go to the wrong counter
                    else
                    {
                        smallText.text = "Wrong Check-in Counter\nFor some reason, you lost 30 seconds.\n [Tip: Ask NPC for help]";
                        StartCoroutine(gameManager.ShowThingTemporarily(smallText.gameObject, 2));
                        interactable = false;
                        gameManager.timeRemain -= 30;
                        interact.gameObject.SetActive(false);
                        gameManager.timeRemainText.text = gameManager.displayTime(gameManager.timeRemain);
                        gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
                    }
                }
            }
        }
    }

    // Show interact key [f] in the UI
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not checked in
        if (other.gameObject.CompareTag("Player") && !gameManager.tasks[2])
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
