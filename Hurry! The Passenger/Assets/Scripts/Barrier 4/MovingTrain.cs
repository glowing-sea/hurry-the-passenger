using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrain : MonoBehaviour
{
    public GameObject targetLocReference;
    public float stayDuration;
    public float speed;

    private Vector3 loc1;
    private Vector3 loc2;
    private ObjectState state;

    private GameObject player;

    [SerializeField] private GameObject onBoardTriggerObject;
    private TrainPlayerDetector onBoardTrigger;

    public enum ObjectState
    {
        GetToLoc1,
        StayingAtLoc1,
        GoingToLoc2,
        GetToLoc2,
        StayingAtLoc2,
        GoingToLoc1,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = ObjectState.GetToLoc1;
        loc1 = transform.localPosition;
        loc2 = targetLocReference.transform.localPosition;
        player = GameObject.FindGameObjectWithTag("Player");
        onBoardTrigger = onBoardTriggerObject.GetComponent<TrainPlayerDetector>();
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


    // Update is called once per frame
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
                // rb.velocity = new Vector3(0, 0, speed);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, loc2, Time.deltaTime * speed);


                if (onBoardTrigger.playerOnBoard)
                {
                    Vector3 displacement = (loc2 - transform.localPosition).normalized * Time.deltaTime * speed;
                    player.transform.localPosition += displacement;
                }

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

                if (onBoardTrigger.playerOnBoard)
                {
                    Vector3 displacement = (loc1 - transform.localPosition).normalized * Time.deltaTime * speed;
                    player.transform.localPosition += displacement;
                }
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

