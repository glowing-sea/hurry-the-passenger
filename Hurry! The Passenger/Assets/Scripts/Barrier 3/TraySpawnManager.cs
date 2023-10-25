using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySpawnManager : MonoBehaviour
{
    public GameObject sampleTray;
    public GameObject[] trays;
    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpqwnRandomTrays(sampleTray.transform.position, sampleTray.transform.rotation, spawnDelay));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Spawn cars given a time interval
    IEnumerator SpqwnRandomTrays(Vector3 spawnPos, Quaternion spawnRot, float spawnDelay)
    {
        int trayIndex;
        while (true)
        {
            trayIndex = Random.Range(0, trays.Length);
            Instantiate(trays[trayIndex], spawnPos, spawnRot, transform);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
