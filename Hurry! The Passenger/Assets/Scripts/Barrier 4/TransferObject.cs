using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AutoDoor;

// A script control an object to moving between its initial location and the target location
public class TransferObject : MonoBehaviour
{
    public GameObject targetLocReference;
    public float stayDuration;
    public float speed;

    private Vector3 loc1;
    private Vector3 loc2;
    private ObjectState state;

    public enum ObjectState
    {
        GetToLoc1,
        StayingAtLoc1,
        GoingToLoc2,
        GetToLoc2,
        StayingAtLoc2,
        GoingToLoc1,
    }

    void Start()
    {
        state = ObjectState.GetToLoc1;
        loc1 = transform.localPosition;
        loc2 = targetLocReference.transform.localPosition;
    }

    IEnumerator StayAtLoc1ForSomeTime()
    {
        yield return new WaitForSeconds(stayDuration);
        state = ObjectState.GoingToLoc2;
    }

    IEnumerator StayAtLoc2ForSomeTime()
    {
        yield return new WaitForSeconds(stayDuration);
        state = ObjectState.GoingToLoc1;
    }


    void FixedUpdate()
    {
        // Case 1: Get to loc1, wait for some time
        if (state == ObjectState.GetToLoc1)
        {
            StartCoroutine(StayAtLoc1ForSomeTime());
            state = ObjectState.StayingAtLoc1;
        }

        // Case 2: Waiting finished, going to loc 2
        else if (state == ObjectState.GoingToLoc2)
        {
            // Door is moving to its opened position
            if (transform.localPosition != loc2)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, loc2, Time.deltaTime * speed);
            }
            // Door has got to its opened position
            else
            {
                // Update the door state
                state = ObjectState.GetToLoc2;
            }
        }

        // Case 3: Get to loc2, wait for some time
        else if (state == ObjectState.GetToLoc2)
        {
            StartCoroutine(StayAtLoc2ForSomeTime());
            state = ObjectState.StayingAtLoc2;
        }

        // Case 4:  Waiting finished, going to loc 1
        else if (state == ObjectState.GoingToLoc1)
        {
            // Door is moving to its closed position
            if (transform.localPosition != loc1)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, loc1, Time.deltaTime * speed);
            }
            // Door has got to its closed position
            else
            {
                // Update the door state
                state = ObjectState.GetToLoc1;
            }

        }
    }
}
