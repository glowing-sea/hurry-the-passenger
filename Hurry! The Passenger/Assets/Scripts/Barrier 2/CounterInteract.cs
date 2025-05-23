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
            text = "Hey, I'm checking in this flight. [shows the ticket]"
        };
        gameManager.TimeDecrese(30);
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "My apologies, but this is not the counter for your flight. Please find the right counter.",
            sound = 2
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "(Welps. There goes 30 seconds of my time.)"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "(I should ask someone at the airport for help.)"
        };
        interactable = true;
        interact.gameObject.SetActive(true);
    }

    private IEnumerator<DialogSystem.Dialog> DialogsRightCounterNoBaggage()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "Hey, I'm checking in this flight. [shows the ticket]"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Alright.[stares at the baggage] Well, my apologies, you need to organise your baggage first as they are overweighted.",
            sound = 2
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "What? Where can I do that?"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Baggage Organiser is on the right side of the Airport Entrance."
        };
        interactable = true;
        interact.gameObject.SetActive(true);
    }

    private IEnumerator<DialogSystem.Dialog> DialogsRightCounter()
    {
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "Hey, I'm checking in this flight. [shows the ticket]"
        };
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "Alright. Have you checked your baggage? [stares at the baggage] Nice. Now please wait for a moment."
        };
        gameManager.CompleteTask(checkInTask);
        yield return new DialogSystem.Dialog
        {
            name = "Counter Staff",
            text = "[operates the computer] Alright. You are good to go. Please hurry to the Domestic Departure."
        };
        yield return new DialogSystem.Dialog
        {
            name = "Me",
            text = "Domestic Departure. Got it. (Time is running out. I should hurry.)"
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
