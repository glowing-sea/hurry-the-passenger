using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DialogSystem;

// This is a script attaching to a trigger object to detect if the player has finish all
// task required to pass a barrier. Optionally, remove a block to let the player pass through
public class BarrierFinishTrigger : MonoBehaviour
{
    public bool destroyAfterUse = true;
    [SerializeField] private List<PlayerTask> requiredTasks;
    [SerializeField] private PlayerTask finishedTask;

    // Script
    private GameManager gameManager; // reference to the game manager script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
    }

    // Call when the player touch it
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        // Check if the player has completed all the required tasks
        if (requiredTasks.All((task) => gameManager.GetTaskState(task).isComplete))
        {
            gameManager.CompleteTask(finishedTask);
            if (destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
