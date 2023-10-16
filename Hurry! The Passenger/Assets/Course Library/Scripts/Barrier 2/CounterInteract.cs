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


    // Script
    private GameManager gameManager; // reference to the game manager script
    public BaggageOrganiserInteract baggageOrganiser; // reference to BaggageOrganiserInteract script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
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
                        gameManager.dialogSystem.StartDialog(DialogsRightCounter());
                        interactable = false;
                        interact.gameObject.SetActive(false);
                        gameManager.tasks[2] = true;
                        gameManager.UpdateNotesMenu();
                        gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
                    }
                    // Player goes to the right counter but hasn't organized baggage
                    else if (rightCounter)
                    {
                        gameManager.dialogSystem.StartDialog(DialogsRightCounterNoBaggage());
                        gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
                    }
                    // Player go to the wrong counter
                    else
                    {
                        gameManager.dialogSystem.StartDialog(DialogsWrongCounter());
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

    private IEnumerator<DialogSystem.Dialog> DialogsWrongCounter()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Wrong Check-in Counter\nFor some reason, you lost 30 seconds.\n [Tip: Ask NPC for help]"
        };
    }

    private IEnumerator<DialogSystem.Dialog> DialogsRightCounterNoBaggage()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Please organise your baggage first!\nBaggae Organiser is on the right side of the Airport Entrance"
        };
    }

    private IEnumerator<DialogSystem.Dialog> DialogsRightCounter()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Check-in Completed!\n Please go to the International Departure!\nTime is Running Out!"
        };
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
