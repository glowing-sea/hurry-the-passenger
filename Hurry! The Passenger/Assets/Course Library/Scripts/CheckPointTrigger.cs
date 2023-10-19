using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckPointTrigger : MonoBehaviour
{
    public bool destroyAfterUse;
    
    [Tooltip("Task to mark as completed, optional")]
    public PlayerTask completedTask;

    public string checkPointSceneName;

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

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
