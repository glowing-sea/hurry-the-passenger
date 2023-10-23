using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The script that will run when the player enter a new barrier
// It should be placed in the object in the scene in the new barrier (not the previous barrier)
[RequireComponent(typeof(Collider))]
public class CheckPointTrigger : MonoBehaviour
{
    public bool destroyAfterUse = true;
    
    [Tooltip("Task to mark as completed, optional")]
    public PlayerTask completedTask;

    public string checkPointSceneName;

    private TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager gameManager = GameManager.instance;

        // Set the task to complete
        if (completedTask)
        {
            gameManager.CompleteTask(completedTask);
        }

        // Set checkpoint
        gameManager.ReachNewBarrier(checkPointSceneName);

        // Show auto save indicator
        StartCoroutine(gameManager.ShowThingTemporarily(gameManager.mainUI.autoSavingIndicator, 2));


        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
