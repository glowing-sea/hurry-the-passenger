using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script to an invisible object located at the airport gate to check if the play has finish barrier 1
public class BarrierOneFinishTrigger : MonoBehaviour
{
    [SerializeField] private PlayerTask finishedTask;

    // Player passed through the airport gate. Means that player has complete Barrier 1
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gameManager = GameManager.instance;

            // Set the task to complete
            gameManager.CompleteTask(finishedTask);

            // Set checkpoint to the next scene
            gameManager.ReachSceneCheckPoint("Barrier 2");
        }
    }
}