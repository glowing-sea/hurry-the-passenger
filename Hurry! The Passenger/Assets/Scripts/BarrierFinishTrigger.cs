using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DialogSystem;
using Dialog = DialogSystem.Dialog;

// This is a script attaching to a trigger object to detect if the player has finish all
// task required to pass a barrier. Optionally, remove a block to let the player pass through
public class BarrierFinishTrigger : MonoBehaviour
{
    public bool destroyAfterUse = true;
    [SerializeField] private List<PlayerTask> requiredTasks;
    [SerializeField] private PlayerTask finishedTask;
    [SerializeField] private List<Dialog> enterDialogue;
    [SerializeField] private List<Dialog> exitDialogue;
    [SerializeField] private List<Dialog> succeedDialogue;
    [SerializeField] private List<Dialog> failDialogue;

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
        } else
        {
            gameManager.dialogSystem.StartDialog(Dialog(false));
        }
    }

    IEnumerator<Dialog> Dialog(bool success)
    {
        // yield return new Dialog("Airport Security", "Only passengers with a valid boarding pass may go through.");
        foreach (Dialog dialog in enterDialogue)
        {
            yield return dialog;
        }

        if (success)
        {
            foreach (Dialog dialog in succeedDialogue)
            {
                yield return dialog;
            }
            gameManager.sfxPlayer.PlayOneShot(gameManager.taskComplete, 1.0f);
        }
        else
        {
            foreach (Dialog dialog in failDialogue)
            {
                yield return dialog;
            }
            gameManager.sfxPlayer.PlayOneShot(gameManager.somethingWrong, 1.0f);
        }

        foreach (Dialog dialog in exitDialogue)
        {
            yield return dialog;
        }
    }
}
