using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityScannerInteract : MonoBehaviour
{
    GameObject interactPrompt;
    bool interactable;

    void Start()
    {
        interactPrompt = GameManager.instance.mainUI.interactPrompt;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameState.Running && interactable && Input.GetKeyDown(KeyCode.F))
        {
            // TODO: start security check
        }
    }

    void OnTriggerEnter()
    {
        interactable = true;
        interactPrompt.SetActive(true);
    }

    void OnTriggerExit()
    {
        interactable = false;
        interactPrompt.SetActive(false);
    }
}
