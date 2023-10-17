using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InternationalDepatureInteract : MonoBehaviour
{
    [SerializeField] private List<PlayerTask> requiredTasks;
    [SerializeField] private PlayerTask finishedTask;

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
        if (!other.gameObject.CompareTag("Player")) return;

        // Check if the player has completed all the required tasks
        if (requiredTasks.All((task) => gameManager.GetTaskState(task).isComplete))
        {
            // All tasks are complete. the game is finished
            gameManager.GameFinished();
        }
        else
        {
            largeText.text = "Some Tasks are\nincomplete!";
            StartCoroutine(gameManager.ShowThingTemporarily(largeText.gameObject, 2));
            gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
        }
    }
}
