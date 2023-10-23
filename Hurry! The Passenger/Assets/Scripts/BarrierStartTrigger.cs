using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// The script that will run when the player enter a new barrier
// It should be placed in the object in the scene in the new barrier (not the previous barrier)
[RequireComponent(typeof(Collider))]
public class BarrierStartTrigger : MonoBehaviour
{
    public bool destroyAfterUse = true;
    public string sceneName;

    private TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager gameManager = GameManager.instance;

        // Set checkpoint
        gameManager.ReachNewBarrier(sceneName);

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
