using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public enum DoorState
    {
        Opening,
        Closing,
        Opened,
        Closed,
    }


    public bool isRightDoor; // whether right or left door
    public DoorState doorState; // current state of the door


    private float stayOpenedTime; // The time the door stay open
    private float doorSpeed; // spped of opening or closing


    private float doorX = 22.7f;
    private float doorY = 2.5f;
    public float openedDoorZ; /// where the door locates when it is open
    public float closedDoorZ;

    private AutoDoorTrigger autoDoorTrigger;


    // Start is called before the first frame update
    void Start()
    {
        autoDoorTrigger = GameObject.Find("Door Controller").GetComponent<AutoDoorTrigger>();
        doorSpeed = autoDoorTrigger.doorSpeed;

        if (!isRightDoor)
        {
            openedDoorZ *= -1;
            closedDoorZ *= -1;
            doorSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Open the door
        if (doorState == DoorState.Opening)
        {
            // Door is moving to its opened position
            if (Mathf.Abs(transform.localPosition.z) < openedDoorZ)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * doorSpeed, Space.Self);
            }
            // Door has got to its opened position
            else
            {
                // Adjust right door's opened position
                if (isRightDoor)
                {
                    transform.localPosition = new Vector3(doorX, doorY, openedDoorZ);
                }
                // Adjust left door's opened positon
                else
                {
                    transform.localPosition = new Vector3(doorX, doorY, openedDoorZ * -1);
                }
                // Update the door state
                doorState = DoorState.Opened;
            }

        }
        // Close the door
        else if (doorState == DoorState.Closing)
        {
            // Door is moving to itstis closed position
            if (Mathf.Abs(transform.localPosition.z) > closedDoorZ)
            {
                transform.Translate(Vector3.back * Time.deltaTime * doorSpeed, Space.Self);
            }
            // Door has got to its closed position
            else
            {
                // Adjust right door's closed positon
                if (isRightDoor)
                {
                    transform.localPosition = new Vector3(doorX, doorY, closedDoorZ);
                }
                // Adjust left door's closed position
                else
                {
                    transform.localPosition = new Vector3(doorX, doorY, closedDoorZ * -1);
                }
                // Update the door state
                doorState = DoorState.Closed;
            }

        }
        // Keep the door opened for some time
        else if (doorState == DoorState.Opened)
        {
            StartCoroutine(StayOpenedForSomeTime());
        }
    }

    // Keep the Open state for some time and then turn it into Closing state.
    IEnumerator StayOpenedForSomeTime()
    {
        stayOpenedTime = autoDoorTrigger.stayOpenedTime;
        yield return new WaitForSeconds(stayOpenedTime);
        doorState = DoorState.Closing;
        Debug.Log(stayOpenedTime);
    }
}
