using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    [SerializeField] private List<Dialog> enterDialogue = new();
    [SerializeField] private List<Dialog> exitDialogue = new();
    [SerializeField] private List<Dialog> succeedDialogue = new();
    [SerializeField] private List<Dialog> failDialogue = new();
    [SerializeField] private bool lastBarrier = false; // finishing this will complete the game

    // Script
    private GameManager gameManager; // reference to the game manager script

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
    }

    // Wait for the playing of dialog to complete
    private bool dialogueStartPlaying = false;
    void Update()
    {
        // All dialogue has been played and the game state is back to running
        if (dialogueStartPlaying & gameManager.gameState == GameState.Running) {
            if (lastBarrier)
                gameManager.GameFinished();
            if (destroyAfterUse)
                Destroy(gameObject);
        }   
    }

    // Call when the player touch it
    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("Player")) return;

        // Check if the player has completed all the required tasks
        if (requiredTasks.All((task) => gameManager.GetTaskState(task).isComplete))
        {

            if (finishedTask != null)
                gameManager.CompleteTask(finishedTask);

            gameManager.dialogSystem.StartDialog(Dialog(true));
            dialogueStartPlaying = true;

        } else
        {
            Debug.Log("Some task in Barrier is incompleted");
            DialogTrigger.StartDialog(enterDialogue);
            DialogTrigger.StartDialog(failDialogue);
            //DialogTrigger.StartDialog(exitDialogue);
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
