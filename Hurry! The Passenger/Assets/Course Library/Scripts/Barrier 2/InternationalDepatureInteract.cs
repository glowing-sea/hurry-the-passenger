using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InternationalDepatureInteract : MonoBehaviour
{
    // UI
    TextMeshProUGUI largeText;


    // Script
    private GameManager gameManager; // reference to the game manager script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        largeText = gameManager.mainUI.largeArbitraryText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Call when the player touch it
    private void OnTriggerEnter(Collider other)
    {
        bool[] tasks = gameManager.tasks;
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                // Some tasks are incomplete
                if (!tasks[i])
                {
                    largeText.text = "Some Tasks are\nincomplete!";
                    StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
                    gameManager.soundEffect.PlayOneShot(gameManager.somethingWrong, 1.0f);
                    return;

                }
            }
            // All tasks are complete. the game is finished
            gameManager.GameFinished();
            return;
        }
    }
}
