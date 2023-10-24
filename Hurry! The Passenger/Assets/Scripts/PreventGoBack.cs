using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventGoBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var gameManager = GameManager.instance;
        SpawnPoint spawnPoint = SpawnPoint.FindInScene(gameManager.currentSceneName);
        gameManager.TeleportPlayer(spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
