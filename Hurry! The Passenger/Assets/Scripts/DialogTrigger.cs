using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog = DialogSystem.Dialog;

[RequireComponent(typeof(Collider))]
public class DialogTrigger : MonoBehaviour
{
    public bool destroyAfterUse = false;
    public List<Dialog> dialogs = new();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player") return;

        StartCoroutine(WaitForRunningState());
    }
    
    // Wait for game state to become running
    IEnumerator WaitForRunningState()
    {
        while (GameManager.instance.gameState != GameState.Running)
        {
            yield return null;
        }

        GameManager.instance.dialogSystem.StartDialog(dialogs.GetEnumerator());

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
