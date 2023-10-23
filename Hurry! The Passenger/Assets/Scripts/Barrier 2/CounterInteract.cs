using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterInteract : MonoBehaviour
{
    [SerializeField] private PlayerTask baggageTask;
    [SerializeField] private PlayerTask checkInTask;

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
        // If player nearby have not checked in
        if (interactable && !gameManager.GetTaskState(checkInTask).isComplete)
        {
            // If the player press F
            if (Input.GetKeyDown(KeyCode.F))
            {
                    // Player goes to the right counter and has organized baggage
                if (rightCounter && gameManager.GetTaskState(baggageTask).isComplete)
                {
                    gameManager.dialogSystem.StartDialog(DialogsRightCounter());
                    interactable = false;
                    interact.gameObject.SetActive(false);
                }
                // Player goes to the right counter but hasn't organized baggage
                else if (rightCounter)
                {
                    gameManager.dialogSystem.StartDialog(DialogsRightCounterNoBaggage());
                    interactable = false;
                    interact.gameObject.SetActive(false);
                }
                // Player go to the wrong counter
                else
                {
                    gameManager.dialogSystem.StartDialog(DialogsWrongCounter());
                    interactable = false;
                    interact.gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator<DialogSystem.Dialog> DialogsWrongCounter()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "Hey, I'm checking-in to this flight. [shows the ticket]"
        };
        gameManager.timeRemain -= 30;
        gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "My apologies, but this is not the counter for your flight. Please find the right counter."
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "(Welps. There goes 30 seconds of my time.)"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "(I should have asked someone at the airport for help.)"
        };
    }

    private IEnumerator<DialogSystem.Dialog> DialogsRightCounterNoBaggage()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "Hey, I'm checking-in to this flight. [shows the ticket]"
        };
        gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Alright. Do you have your checked baggage? [stares at the baggage] Well, my apologies, but you need to organised your baggage first."
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "What? Where can I do that?"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Baggae Organiser is on the right side of the Airport Entrance."
        };
    }

    private IEnumerator<DialogSystem.Dialog> DialogsRightCounter()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "Hey, I'm checking-in to this flight. [shows the ticket]"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Alright. Do you have your checked baggage? [stares at the baggage] Nice. Now please wait for a moment."
        };
        gameManager.CompleteTask(checkInTask);
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "[operates the computer] Alright. You are good to go. Please go to the International Departure."
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "International Departure. Got it. (Time is running out. I should hurry.)"
        };
    }

    // Show interact key [f] in the UI
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not checked in
        if (other.gameObject.CompareTag("Player") && !gameManager.GetTaskState(checkInTask).isComplete)
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
