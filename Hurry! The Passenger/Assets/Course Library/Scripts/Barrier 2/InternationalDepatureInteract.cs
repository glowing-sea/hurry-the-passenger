using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Dialog = DialogSystem.Dialog;

public class InternationalDepatureInteract : MonoBehaviour
{
    [SerializeField] private List<PlayerTask> requiredTasks;
    [SerializeField] private PlayerTask finishedTask;
    [SerializeField] private GameObject blocker;

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
            gameManager.dialogSystem.StartDialog(Dialog(true));
        }
        else
        {
            gameManager.dialogSystem.StartDialog(Dialog(false));
        }
    }

    IEnumerator<Dialog> Dialog(bool success)
    {
        yield return new Dialog("Airport Security", "Only passengers with a valid boarding pass may go through.");
        if (!success)
        {
            gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
            yield return new Dialog("Me", "Of course. (I need to go check-in first)");
            yield break;
        }

        yield return new Dialog("Me", "[shows boarding pass]");
        gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
        yield return new Dialog("Airport Security", "[gestures approval]");
        Destroy(gameObject);
    }
}
