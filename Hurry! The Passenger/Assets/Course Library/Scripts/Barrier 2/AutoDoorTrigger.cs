using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorTrigger : MonoBehaviour
{
    private AutoDoor autoDoorRight;
    private AutoDoor autoDoorLeft;
    public float doorSpeed;
    public float[] stayOpenedTimes;
    public float stayOpenedTime;
    private int openedTimes = 0;

    // Start is called before the first frame update
    void Start()
    {
        autoDoorRight = GameObject.Find("Door Right").GetComponent<AutoDoor>(); // get reference
        autoDoorLeft = GameObject.Find("Door Left").GetComponent<AutoDoor>(); // get reference
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Open the doors
        autoDoorRight.doorState = AutoDoor.DoorState.Opening;
        autoDoorLeft.doorState = AutoDoor.DoorState.Opening;

        if (openedTimes % 5 == 4)
        {
            stayOpenedTime = 0.5f;
        }
        else
        {
            int stayOpenedTimesIdx = Random.Range(0, stayOpenedTimes.Length);
            stayOpenedTime = stayOpenedTimes[stayOpenedTimesIdx];
        }

        openedTimes++;
    }
}
