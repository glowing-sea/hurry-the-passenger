using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecurityCheckMinigame : MonoBehaviour
{
    public GameObject sampleItem; // the item indicate where to drop other items.

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
    public TextMeshProUGUI itemNumText;

    // A list of item prefabs
    public GameObject[] items;

    // Number of iten need to scanned
    public int itemNum;



    // Helper property to get the instance of the security check Minigame
    public static SecurityCheckMinigame instance
    {
        get
        {
            GameObject obj = GameObject.Find("Security Check (Minigame)");
            if (obj != null)
                return obj.GetComponent<SecurityCheckMinigame>();
            else
                throw new System.NullReferenceException("Object of Security Check (Minigame) not found");
        }
    }

    void Start()
    {
        itemNumText.text = "Item Left: " + itemNum;
        gameManager = GameManager.instance; // get reference
        interactPrompt = GameManager.instance.mainUI.interactPrompt;
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
            gameManager.mainUI.autoSavingIndicator.SetActive(false);
        }
    }

    public void DropItem()
    {
        GameObject instance;
        instance = Instantiate(items[0], sampleItem.transform.position, sampleItem.transform.rotation, transform);
        ItemAttributes attributes = instance.GetComponent<ItemAttributes>();
        attributes.itemType = ItemType.Normal;
    }


    // Show [F] when the player close to it
    private void OnTriggerEnter(Collider other)
    {
        // if the player close to this object and has not finished organising their baggage
        if (other.gameObject.CompareTag("Player") && !gameManager.GetTaskState(securityCheckTask).isComplete)
        {
            interactable = true;
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit()
    {
        interactable = false;
        interactPrompt.SetActive(false);
    }

    public void DecrementItemNum()
    {
        itemNum -= 1;
        itemNumText.text = "Item Left: " + itemNum;
        if (itemNum == 0)
        {
            MiniGameFinish();
        }
    }

    private void MiniGameFinish()
    {
        Destroy(securiyBarrier);
        gameManager.CompleteTask(securityCheckTask);
        TextMeshProUGUI largeText = gameManager.mainUI.largeArbitraryText.GetComponent<TextMeshProUGUI>();
        largeText.text = "Security Check Complete!";
        StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
        ExitGame();
    }

    // When the player want to exit the baggage organisation view
    public void ExitGame()
    {
        securityCheckMenu.SetActive(false); // close special menu
        if(!gameManager.GetTaskState(securityCheckTask).isComplete)
            interactPrompt.gameObject.SetActive(true); // reopen interact prompt
        gameManager.gameState = GameState.Running; // reset game state
        securityCheckCamera.depth = -1; // bring security camera back
        gameManager.mainUI.minimap.SetActive(true); // reopen minimap
        gameManager.mainUI.taskIcon.SetActive(true);
        gameManager.mainUI.guideIcon.SetActive(true);
        gameManager.mainUI.pauseIcon.SetActive(true);

        // Clean Up
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Item"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}

