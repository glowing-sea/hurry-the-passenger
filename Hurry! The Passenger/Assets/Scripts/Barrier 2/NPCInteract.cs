using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    // The dialogs the NPC is going to have
    public List<DialogSystem.Dialog> dialogs;

    // Interactable object variable
    private TextMeshProUGUI interact;
    private bool interactable;

    // Script
    private GameManager gameManager; // reference to the game manager script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
    }

    bool dialogueStartPlaying;

    // Update is called once per frame
    void Update()
    {
        // If the player nearby
        if (interactable)
        {
            // If the player press F
            if (Input.GetKeyDown(KeyCode.F))
            {
                gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
                dialogueStartPlaying = true;
                interact.gameObject.SetActive(false);
                interactable = false;
                gameManager.dialogSystem.StartDialog(dialogs.GetEnumerator());
            }
        }

        // Dialog playing is finish, make it interactable again
        if (dialogueStartPlaying & gameManager.gameState == GameState.Running)
        {
            interact.gameObject.SetActive(true);
            interactable = true;
            dialogueStartPlaying = false;
        }
    }


    // Show interact key [f] in the UI
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this NPC
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
