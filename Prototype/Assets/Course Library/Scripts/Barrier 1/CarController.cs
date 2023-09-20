using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script for cars
// Move car forward
// Delete car if it go out of the boundary
// Check if the car collide with the player
public class CarController : MonoBehaviour
{
    public float speed;

    // When to destroy
    private float rightBound = 65;
    private float leftBound = -80;

    // Car may stop for a while a run for a while
    public bool stop = false;
    public float stopTime = 0;

    // Script
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // get reference to Game Manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (stopTime > 0)
        {
            // Control start and stop
            StartCoroutine(StopTheCar(stopTime));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            // controller the projectile moving forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        // when an object goes out of the boundary, destroy it
        if (transform.position.z < leftBound || transform.position.z > rightBound)
        {
            Destroy(gameObject);
        }

    }

    // Stop the car for a while and run it for a while
    IEnumerator StopTheCar(float stopTime)
    {
        while (true)
        {
            stop = false;
            yield return new WaitForSeconds(stopTime);
            stop = true;
            yield return new WaitForSeconds(stopTime);
        }
    }

    // When the car collide with the player, the game fails
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && gameManager.gameState != -1)
        {
            gameManager.explosionParticle.Play();
            gameManager.GameOver();
        }
    }
}
