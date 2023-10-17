using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script to an invisible object located at the airport gate to check if the play has finish barrier 1
public class BarrierOneFinishTrigger : MonoBehaviour
{
    // Player passed through the airport gate. Means that player has complete Barrier 1
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gameManager = GameManager.instance;

            // Mark task complete
            if (gameManager.tasks[0] == false)
            {
                gameManager.tasks[0] = true;
                gameManager.UpdateNotesMenu();
                gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
            }

            // Set checkpoint to the next scene
            gameManager.SetContinueScene("Barrier 2");
        }
    }
}