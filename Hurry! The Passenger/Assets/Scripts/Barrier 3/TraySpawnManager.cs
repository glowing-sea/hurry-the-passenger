using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySpawnManager : MonoBehaviour
{
    public GameObject[] trays;
    private Vector3 spawnPos;
    private Quaternion spawnRot;
    private float speed;
    private float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = new Vector3(-63.21f, 0.2f, 101.06f);
        spawnRot = Quaternion.Euler(0, 90, 0);
        speed = 6f;
        spawnDelay = 4;
        StartCoroutine(SpqwnRandomTrays(spawnPos, spawnRot, speed, spawnDelay));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Spawn cars given a time interval
    IEnumerator SpqwnRandomTrays(Vector3 spawnPos, Quaternion spawnRot, float speed, float spawnDelay)
    {
        GameObject instance;
        int trayIndex;
        while (true)
        {
            trayIndex = Random.Range(0, trays.Length);
            instance = Instantiate(trays[trayIndex], spawnPos, spawnRot, transform);
            instance.GetComponent<MovingTray>().speed = speed;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
