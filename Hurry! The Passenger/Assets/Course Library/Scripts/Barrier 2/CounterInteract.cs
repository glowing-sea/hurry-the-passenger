using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterInteract : MonoBehaviour
{
    // Interactable object variable
    public TextMeshProUGUI interact;
    private bool interactable;

    public bool rightCounter;

    public TextMeshProUGUI largeText;
    public TextMeshProUGUI smallText;


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
        // If have not checked in
        if (!gameManager.tasks[2])
        {
            // If the player nearby
            if (interactable)
            {
                // If the player press F
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Player go to the right counter
                    if (rightCounter)
                    {
                        largeText.text = "Check-in Coompleted!";
                        StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
                        interactable = false;
                        interact.gameObject.SetActive(false);
                        gameManager.tasks[2] = true;
                        gameManager.UpdateNotesMenu();
                        gameManager.soundEffect.PlayOneShot(gameManager.taskComplete, 1.0f);
                    }
                    // Player go to the wrong counter
                    else
                    {
                        smallText.text = "Wrong Check-in Counter\nFor some reason, you lost 30 seconds";
                        StartCoroutine(gameManager.ShowThingTemporarily(smallText.gameObject, 2));
                        interactable = false;
                        gameManager.timeRemain -= 30;
                        interact.gameObject.SetActive(false);
                        gameManager.timeRemainText.text = gameManager.displayTime(gameManager.timeRemain);
                        gameManager.soundEffect.PlayOneShot(gameManager.somethingWrong, 1.0f);
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
