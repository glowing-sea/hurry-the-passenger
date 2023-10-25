using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityCheckInteract : MonoBehaviour
{
    public float conveyorSpeed;

    // Special Camera for playing the security check minigame
    public Camera securityCheckCamera;

    // The barrier to be destroy after the security check is completed
    public GameObject securiyBarrier;

    // The task to be finished in this minigame
    [SerializeField] PlayerTask securityCheckTask;

    // Interact Text
    GameObject interactPrompt;
    bool interactable;

    // Script
    private GameManager gameManager; // reference to the game manager script

    // UI
    public GameObject securityCheckMenu;

    // Position and rotation to drop a item
    private Vector3 spawnPos;
    private Quaternion spawnRot;

    // A list of item prefabs
    public GameObject[] items;

    void Start()
    {
        gameManager = GameManager.instance; // get reference
        interactPrompt = GameManager.instance.mainUI.interactPrompt;
        spawnPos = new Vector3(-80f, 5.8f, 101.06f);
        spawnRot = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.F) && GameManager.instance.gameState == GameState.Running && !gameManager.GetTaskState(securityCheckTask).isComplete)
        {
            securityCheckMenu.SetActive(true); // open special menu
            interactPrompt.gameObject.SetActive(false); // close interact prompt
            gameManager.gameState = GameState.LeavingMainScene; // go into special game state that player cannot move
            securityCheckCamera.depth = 1; // bring security camera forward
            gameManager.staminaGauge.gameObject.SetActive(false); // hide stamina if player is running
            gameManager.mainUI.minimap.SetActive(false); // hide minimap
            gameManager.mainUI.taskIcon.SetActive(false);
            gameManager.mainUI.guideIcon.SetActive(false);
            gameManager.mainUI.pauseIcon.SetActive(false);
        }
    }

    public void DropItem()
    {
        GameObject instance;
        instance = Instantiate(items[0], spawnPos, spawnRot, transform);

    }


    // Show [F] when the player close to it
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not finished organising their baggage
        if (other.gameObject.CompareTag("Player"))
        {
            interactable = true;
            interactPrompt.SetActive(true);
        }
    }

    // When the player want to exit the baggage organisation view
    public void ExitButton()
    {
        securityCheckMenu.SetActive(false); // close special menu
        interactPrompt.gameObject.SetActive(true); // reopen interact prompt
        gameManager.gameState = GameState.Running; // reset game state
        securityCheckCamera.depth = -1; // bring security camera back
        gameManager.mainUI.minimap.SetActive(true); // reopen minimap
        gameManager.mainUI.taskIcon.SetActive(true);
        gameManager.mainUI.guideIcon.SetActive(true);
        gameManager.mainUI.pauseIcon.SetActive(true);
        Destroy(securiyBarrier);
        gameManager.CompleteTask(securityCheckTask);
    }


    void OnTriggerExit()
    {
        interactable = false;
        interactPrompt.SetActive(false);
    }
}
