using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class CheckPointTrigger : MonoBehaviour
{
    public bool destroyAfterUse;
    
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
        gameManager.ReachSceneCheckPoint(checkPointSceneName);

        // Show auto save indicator
        StartCoroutine(gameManager.ShowThingTemporarily(gameManager.mainUI.autoSavingIndicator, 2));


        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
