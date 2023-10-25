using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Linq;

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

    public int timePunishment;

    // Script
    private GameManager gameManager; // reference to the game manager script

    // UI
    public GameObject securityCheckMenu;
    public TextMeshProUGUI itemLeftText;
    public TextMeshProUGUI timeDecreasePunish;
    public Button dropItemButton;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI itemIndicator;
    public GameObject guideMenu;

    // A list of item prefabs
    public GameObject item;

    // Number of iten need to scanned;
    private int itemNum;
    [SerializeField] private int itemNumNormal;
    [SerializeField] private int itemNumLiquid;
    [SerializeField] private int itemNumDanger;
    private int itemLeft;

    // Have an item ready to be scanned
    private GameObject itemToBeScaned = null;
    public Queue<ItemType> itemsToBeReScanned = new();
    public Queue<ItemType> itemsToBeScanned = new();
    public Queue<ItemType> itemsToBeScannedCopy = new();

    private bool dropItemCoolDown = true;

    // Texture
    // Change items when it is placed in different bags

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
        itemNum = itemNumNormal + itemNumLiquid + itemNumDanger;
        itemLeft = itemNum;
        itemLeftText.text = "Item Left: " + itemLeft;
        gameManager = GameManager.instance; // get reference
        interactPrompt = GameManager.instance.mainUI.interactPrompt;

        List<ItemType> items = new();

        int i;
        for (i = 0; i < itemNumNormal; i++)
        {
            items.Add(ItemType.Normal);
        }
        for (i = 0; i < itemNumLiquid; i++)
        {
            items.Add(ItemType.Liquid);
        }
        for (i = 0; i < itemNumDanger; i++)
        {
            items.Add(ItemType.Danger);
        }

        Shuffle(items);

        itemsToBeScannedCopy = new Queue<ItemType>(items);
        itemsToBeScanned = new Queue<ItemType>(itemsToBeScannedCopy);
        Debug.Log(itemsToBeScannedCopy == null);

        OpenGuide();
    }

    public static void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
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

            itemLeft = itemNum;
            itemLeftText.text = "Item Left: " + itemLeft;
            itemsToBeScanned = new Queue<ItemType>(itemsToBeScannedCopy);

            Debug.Log(itemsToBeScanned == null);
        }

        if (dropItemCoolDown && itemToBeScaned == null)
        {
            AddAnItem();
        }

    }

    private void AddAnItem()
    {
        // All item is out for scanning, no new item to grab
        if (itemLeft == CountItem())
            return;

        itemToBeScaned = Instantiate(item, sampleItem.transform.position, sampleItem.transform.rotation, transform);
        itemToBeScaned.GetComponent<Rigidbody>().useGravity = false;
        ItemAttributes attributes = itemToBeScaned.GetComponent<ItemAttributes>();

        // Nothing to be recanned, grab an new items
        if(itemsToBeReScanned.Count == 0)
        {
            attributes.itemType = itemsToBeScanned.Dequeue();
        } else
        {
            attributes.itemType = itemsToBeReScanned.Dequeue();
            attributes.MarkAsRescan();
        }
        itemIndicator.gameObject.SetActive(true);
    }


    public void DropItem()
    {
        if (itemToBeScaned != null)
        {
            itemToBeScaned.GetComponent<Rigidbody>().useGravity = true;
            itemToBeScaned = null;
            dropItemCoolDown = false;
            itemIndicator.gameObject.SetActive(false);
            StartCoroutine(DropItemCoolDown());
        }
    }

    // Count the number of items out for scanning
    private int CountItem()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Item"))
            {
                count += 1;
            }
        }
        return count;
    }

    // Hide control guides after 3 second
    IEnumerator DropItemCoolDown()
    {
        yield return new WaitForSeconds(1f);
        dropItemCoolDown = true;
    }

    public void ShowWarning(string warning)
    {
        warningText.text = warning;
        gameManager.ShowThingTemporarily(warningText, 2);
        GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.somethingWrong, 1f);
    }

    public void EmptyItem()
    {
        switch (itemToBeScaned.GetComponent<ItemAttributes>().itemType)
        {
            case ItemType.Normal:
                ShowWarning("This item does not contain liquid.");
                break;
            case ItemType.Liquid:
                itemToBeScaned.GetComponent<ItemAttributes>().EmptyLiquid();
                GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.taskComplete, 1f);
                break;
            case ItemType.Danger:
                ShowWarning("This item cannot clear the security screening.");
                break;
        }
    }

    public void DiscardItem()
    {
        switch (itemToBeScaned.GetComponent<ItemAttributes>().itemType)
        {
            case ItemType.Normal:
                ShowWarning("This item should be retained, as it can clear the security screening.");

                break;
            case ItemType.Liquid:
                ShowWarning("This item should be retained as it can clear the security check after emptying its liquid.");
                break;
            case ItemType.Danger:
                DecrementItemLeft();
                GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.taskComplete, 1f);
                Destroy(itemToBeScaned);
                itemToBeScaned = null;
                AddAnItem();
                break;
        }
    }

    public void TimeDecreasePunish()
    {
        gameManager.TimeDecrese(timePunishment);
        gameManager.ShowThingTemporarily(timeDecreasePunish, 2);
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

    public void DecrementItemLeft()
    {
        itemLeft -= 1;
        itemLeftText.text = "Item Left: " + itemLeft;
        if (itemLeft == 0)
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
        gameManager.ShowThingTemporarily(largeText, 2);
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
        itemLeft = itemNum;
    }

    public void OpenGuide()
    {
        guideMenu.SetActive(true);
    }

    public void CloseGuide()
    {
        guideMenu.SetActive(false);
    }
}

