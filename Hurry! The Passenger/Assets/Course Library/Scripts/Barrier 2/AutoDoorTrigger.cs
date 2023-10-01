using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AutoDoorTrigger : MonoBehaviour
{
    public List<AutoDoor> doors;
    [SerializeField] private float[] openDurationCandidates; 
    private int openedTimes = 0; // How many times the door has been opened

    private void OnTriggerEnter(Collider other)
    {
        // Open the doors when player enters the trigger
        if (!other.CompareTag("Player")) return;

        // Select a random duration for the door to stay open,
        // but use a fixed duration of 0.5s every 5 times
        float openDuration;
        if (openedTimes % 5 == 4)
        {
            openDuration = 0.5f;
        }
        else
        {
            int stayOpenedTimesIdx = Random.Range(0, openDurationCandidates.Length);
            openDuration = openDurationCandidates[stayOpenedTimesIdx];
        }

        openedTimes++;

        foreach (AutoDoor door in doors)
        {
            door.openDuration = openDuration;
            door.SetOpen(true);
        }
    }
}
