using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPlayerDetector : MonoBehaviour
{
    public bool playerOnBoard = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerOnBoard = true;
        Debug.Log("Onboard");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerOnBoard = false;
        Debug.Log("Get off");
    }
}
