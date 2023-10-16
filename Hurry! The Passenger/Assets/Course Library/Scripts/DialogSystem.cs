using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private GameObject dialogName;
    [SerializeField] private GameObject dialogText;
    [SerializeField] private GameObject dialogMouseIcon;

    private bool active = false;
    private IEnumerator<Dialog> currentDialog;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        dialogBox.SetActive(false);
    }

    void Update()
    {
        if (!active) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceDialog();
        }
    }

    public void StartDialog(IEnumerator<Dialog> dialogIter)
    {
        gameManager.gameState = GameState.Paused;
        active = true;
        dialogBox.SetActive(true);

        currentDialog = dialogIter;
        AdvanceDialog();
    }

    private void AdvanceDialog()
    {
        if (currentDialog.MoveNext())
        {
            // Display the next dialog
            dialogName.GetComponent<TMPro.TextMeshProUGUI>().text = currentDialog.Current.name;
            dialogText.GetComponent<TMPro.TextMeshProUGUI>().text = currentDialog.Current.text;
        }
        else
        {
            // Dialog is over, resume the game
            gameManager.gameState = GameState.Running;
            active = false;
            dialogBox.SetActive(false);

            currentDialog = null;
        }
    }

    [System.Serializable]
    public struct Dialog
    {
        public string name;
        [TextArea] public string text;

        public Dialog(string name, string text)
        {
            this.name = name;
            this.text = text;
        }
    }
}
