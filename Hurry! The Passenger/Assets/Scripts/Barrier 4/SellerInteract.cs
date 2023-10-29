using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellerInteract : MonoBehaviour
{
    // The dialogs the NPC is going to have
    public List<DialogSystem.Dialog> dialogs;
    public List<DialogSystem.Dialog> dialogsBuy;
    public List<DialogSystem.Dialog> dialogsNotBuy;
    public List<DialogSystem.Dialog> dialogsNotBuyNoMoney;
    public List<DialogSystem.Dialog> dialogsSoldOut;
    public SellerType sellerType;
    public int itemPrice;

    // Interactable object variable
    private TextMeshProUGUI interact;
    private bool interactable;

    private Button buyNow;
    private Button maybeLater;

    // Script
    private GameManager gameManager; // reference to the game manager script
    private PlayerController playerController;

    private bool soldOut = false;

    // int dialogueStartPlaying = 0;

    private BuyState buyState = BuyState.BuyerIntro;

    public enum SellerType
    {
        DrinkSeller,
        MapSeller
    }

    private enum BuyState
    {
        BuyerIntro,
        DecideBuyOrNot,
        Buy,
        NotBuy,
        NotBuyNoMoney,
        Leaving,
        NotTrigger
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        buyState = BuyState.NotTrigger;
        gameManager = GameManager.instance; // get reference
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
        buyNow = gameManager.mainUI.buyNow;
        maybeLater = gameManager.mainUI.maybeLater;


        buyNow.onClick.AddListener(buyNowOnClike);
        maybeLater.onClick.AddListener(maybeLaterOnClick);
    }

    void buyNowOnClike()
    {
        if (buyState == BuyState.DecideBuyOrNot)
        {
            if (gameManager.balance >= itemPrice)
            {
                buyState = BuyState.Buy;
                gameManager.mainUI.sellerConfirm.SetActive(false);
                if (sellerType == SellerType.DrinkSeller)
                {
                    playerController.DrinkEnergyDrink();
                }
                else if (sellerType == SellerType.MapSeller)
                {
                    playerController.LargerMap();
                }
                gameManager.UpdateBalance(itemPrice, false);
                soldOut = true;
            } else
            {
                buyState = BuyState.NotBuyNoMoney;
                gameManager.mainUI.sellerConfirm.SetActive(false);
                gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1);
            }
        }
    }

    void maybeLaterOnClick()
    {
        if (buyState == BuyState.DecideBuyOrNot)
        {
            buyState = BuyState.NotBuy;
            gameManager.mainUI.sellerConfirm.SetActive(false);
        }
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
                if (soldOut)
                {
                    gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
                    buyState = BuyState.Leaving;
                    interact.gameObject.SetActive(false);
                    interactable = false;
                    gameManager.dialogSystem.StartDialog(dialogsSoldOut.GetEnumerator());
                }
                else
                {
                    gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
                    buyState = BuyState.BuyerIntro;
                    interact.gameObject.SetActive(false);
                    interactable = false;
                    gameManager.dialogSystem.StartDialog(dialogs.GetEnumerator());
                }
            }
        }

        // Introduction Dialog playing is finish, play choose buy or not buy
        if (buyState == BuyState.BuyerIntro && gameManager.gameState == GameState.Running)
        {
            buyState = BuyState.DecideBuyOrNot;
            gameManager.gameState = GameState.Paused;
            gameManager.mainUI.sellerConfirm.SetActive(true);
        }


        // Buy
        if (buyState == BuyState.Buy)
        {
            gameManager.dialogSystem.StartDialog(dialogsBuy.GetEnumerator());
            buyState = BuyState.Leaving;
        }

        // not buy
        if (buyState == BuyState.NotBuy)
        {
            gameManager.dialogSystem.StartDialog(dialogsNotBuy.GetEnumerator());
            buyState = BuyState.Leaving;
        }

        // no money
        if (buyState == BuyState.NotBuyNoMoney)
        {
            gameManager.dialogSystem.StartDialog(dialogsNotBuyNoMoney.GetEnumerator());
            buyState = BuyState.Leaving;
        }


        // Good Bye Dialog is Finished
        if (buyState == BuyState.Leaving && gameManager.gameState == GameState.Running)
        {
            interact.gameObject.SetActive(true);
            interactable = true;
            buyState = BuyState.NotTrigger;
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
