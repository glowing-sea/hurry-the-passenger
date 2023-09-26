using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    // The words the NPS is going to say
    public string words;

    // Interactable object variable
    public TextMeshProUGUI interact;
    private bool interactable;

    // Conversation Text Box
    public TextMeshProUGUI conversation;
    public GameObject background;

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
        // If the player nearby
        if (interactable)
        {
            // If the player press F
            if (Input.GetKeyDown(KeyCode.F))
            {
                conversation.text = words;
                StartCoroutine(gameManager.ShowThingTemporarily(background, 4));
                gameManager.soundEffect.PlayOneShot(gameManager.taskComplete, 1.0f);
            }
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
