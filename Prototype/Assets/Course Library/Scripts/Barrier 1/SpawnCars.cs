using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public GameObject[] cars;

    // The information about the first spawning spot
    private Vector3 spawnPos1 = new Vector3(121, 0, -75);
    private Quaternion spawnRot1 = Quaternion.Euler(0, 0, 0);
    private float spawnDelay1 = 2;
    private float speed1 = 20;

    // The information about the second spawning spot
    private Vector3 spawnPos2 = new Vector3(130, 0, 60);
    private Quaternion spawnRot2 = Quaternion.Euler(0, 180, 0);
    private float spawnDelay2 = 2;
    private float speed2 = 15;

    // The information about the third spawning spot
    private Vector3 spawnPos3 = new Vector3(81.7f, 0, -75);
    private Quaternion spawnRot3 = Quaternion.Euler(0, 0, 0);
    private float spawnDelay31 = 0.2f;
    private float spawnDelay32 = 1;
    private float speed3 = 60;

    // The information about the fourth spawning spot
    private Vector3 spawnPos4 = new Vector3(92, 0, 60);
    private Quaternion spawnRot4 = Quaternion.Euler(0, 180, 0);
    private float spawnDelay41 = 0.2f;
    private float spawnDelay42 = 1;
    private float speed4 = 60;

    // The information about the fifth spawning spot
    private Vector3 spawnPos5 = new Vector3(45, 0, -75);
    private Quaternion spawnRot5 = Quaternion.Euler(0, 0, 0);
    private float spawnDelay51 = 4f;
    private float spawnDelay52 = 4f;
    private float speed5 = 8;
    private float stopTime5 = 2;

    // The information about the fourth spawning spot
    private Vector3 spawnPos6 = new Vector3(55, 0, 60);
    private Quaternion spawnRot6 = Quaternion.Euler(0, 180, 0);
    private float spawnDelay61 = 0.2f;
    private float spawnDelay62 = 1;
    private float speed6 = 80;



    // Start is called before the first frame update
    void Start()
    {
        // Spawn cars in Road 1
        StartCoroutine(SpqwnRandomCars(spawnPos1, spawnRot1, speed1, spawnDelay1));
        StartCoroutine(SpqwnRandomCars(spawnPos2, spawnRot2, speed2, spawnDelay2));

        // Spawn cars in Road 2
        StartCoroutine(SpqwnRandomCarsPlus(spawnPos3, spawnRot3, speed3, spawnDelay31, spawnDelay32, 0));
        StartCoroutine(SpqwnRandomCarsPlus(spawnPos4, spawnRot4, speed4, spawnDelay41, spawnDelay42, 0));

        // Spawn cars in Road 3
        StartCoroutine(SpqwnRandomCarsPlus(spawnPos5, spawnRot5, speed5, spawnDelay51, spawnDelay52, stopTime5));
        StartCoroutine(SpqwnRandomCarsPlus(spawnPos6, spawnRot6, speed6, spawnDelay61, spawnDelay62, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Spawn cars given a time interval
    IEnumerator SpqwnRandomCars(Vector3 spawnPos, Quaternion spawnRot, float speed, float spawnDelay)
    {
        GameObject instance;
        int carIndex;
        while (true)
        {
            carIndex = Random.Range(0, cars.Length);
            instance = Instantiate(cars[carIndex], spawnPos, spawnRot);
            instance.GetComponent<CarController>().speed = speed;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Spawn cars given a time 2 interval and stop time
    // spawn 4 cars with interval 1, then wait for interval 2 time, repeat
    IEnumerator SpqwnRandomCarsPlus(Vector3 spawnPos, Quaternion spawnRot, float speed, float spawnDelay1, float spawnDelay2, float stopTime)
    {
        GameObject instance;
        int carIndex;
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                carIndex = Random.Range(0, cars.Length);
                instance = Instantiate(cars[carIndex], spawnPos, spawnRot);
                instance.GetComponent<CarController>().speed = speed;
                // make the car run for "stopTime" and stop for "stopTime" if stopTime > 0
                if (stopTime > 0)
                {
                    instance.GetComponent<CarController>().stopTime = stopTime;
                }
                yield return new WaitForSeconds(spawnDelay1);
            }
            yield return new WaitForSeconds(spawnDelay2);
        }
    }
}
