using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoDoor : MonoBehaviour
{
    public enum DoorState
    {
        Opening,
        Closing,
        Opened,
        StayingOpened,
        Closed,
    }

    private TextMeshProUGUI interact;
    private bool interactable;

    // Script
    private GameManager gameManager; // reference to the game manager script

    public List<DialogSystem.Dialog> dialogs;
    public DoorState doorState { get; private set; } // current state of the door

    public float doorSpeed; // spped of opening or closing
    [System.NonSerialized] public float openDuration; // The time the door stay open


    private Vector3 closedPosition;
    public Vector3 openedDisplacement; // The displacement of the door when it is opened
    private Vector3 openedPosition { get { return closedPosition + openedDisplacement; } }


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; // get reference
        interact = gameManager.mainUI.interactPrompt.GetComponent<TextMeshProUGUI>();
        doorState = DoorState.Closed;
        closedPosition = transform.localPosition; // use the saved position as the closed position
    }

    // Update is called once per frame
    void Update()
    {
        // Open the door
        if (doorState == DoorState.Opening)
        {
            // Door is moving to its opened position
            if (transform.localPosition != openedPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, openedPosition, Time.deltaTime * doorSpeed);
            }
            // Door has got to its opened position
            else
            {
                // Update the door state
                doorState = DoorState.Opened;
            }

        }
        // Close the door
        else if (doorState == DoorState.Closing)
        {
            // Door is moving to its closed position
            if (transform.localPosition != closedPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, closedPosition, Time.deltaTime * doorSpeed);
            }
            // Door has got to its closed position
            else
            {
                // Update the door state
                doorState = DoorState.Closed;
            }

        }
        // Keep the door opened for some time
        else if (doorState == DoorState.Opened)
        {
            StartCoroutine(StayOpenedForSomeTime());
            doorState = DoorState.StayingOpened;
        }
    }

    // Keep the Open state for some time and then turn it into Closing state.
    IEnumerator StayOpenedForSomeTime()
    {
        yield return new WaitForSeconds(openDuration);
        SetOpen(false);
    }

    // Change the door state to Opening or Closing
    public void SetOpen(bool open)
    {
        if (open)
        {
            doorState = DoorState.Opening;
        }
        else
        {
            doorState = DoorState.Closing;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the door's opened displacement and position
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, openedDisplacement);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireMesh(meshFilter.sharedMesh, transform.position + openedDisplacement, transform.rotation, transform.localScale);
        }
    }

    //if the player collides with door
    private void OnCollisionEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             gameManager.dialogSystem.StartDialog(dialogs.GetEnumerator());
        }
    }
}
