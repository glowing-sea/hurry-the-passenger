using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AutoDoorTrigger : MonoBehaviour
{
    public List<AutoDoor> doors;
    [SerializeField] private float[] openDurationCandidates;

    private void OnTriggerEnter(Collider other)
    {
        // Open the doors when player enters the trigger
        if (!other.CompareTag("Player")) return;

        // Select a random duration for the door to stay open
        int stayOpenedTimesIdx = Random.Range(0, openDurationCandidates.Length);
        float openDuration = openDurationCandidates[stayOpenedTimesIdx];

        foreach (AutoDoor door in doors)
        {
            door.openDuration = openDuration;
            door.SetOpen(true);
            Debug.Log("Switch to Opening");
        }
    }
}
