using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMPro.TMP_Text messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private System.Action action;

    public static void Show(string message, System.Action action)
    {
        var instance = FindObjectOfType<ConfirmationDialog>();
        if (!instance)
        {
            Debug.LogError("No ConfirmationDialog instance is loaded");
            return;
        }
        instance.messageText.text = message;
        instance.dialogBox.SetActive(true);
        instance.action = action;
    }

    void Start()
    {
        confirmButton.onClick.AddListener(() => 
        {
            dialogBox.SetActive(false);
            action.Invoke();
        });
        cancelButton.onClick.AddListener(() => dialogBox.SetActive(false));
    }
}
