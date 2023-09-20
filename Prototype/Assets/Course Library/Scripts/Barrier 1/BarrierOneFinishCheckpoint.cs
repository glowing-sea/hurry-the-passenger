using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script to an invisible object located at the airport gate to check if the play has finish barrier 1
public class BarrierOneFinishCheckpoint : MonoBehaviour
{
    // Reference to script variables
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Player passed through the airport gate. Means that play has complete Barrier 1
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameManager.tasks[0] == false)
            {
                gameManager.tasks[0] = true;
                gameManager.UpdateNotesMenu();
                gameManager.soundEffect.PlayOneShot(gameManager.taskComplete, 1.0f);
                gameManager.bgm.clip = gameManager.bgm2;
                gameManager.bgm.Play();
                gameManager.roadAmbiance.Stop();
                gameManager.airportAmbiance.Play();
            }
        }
    }
}