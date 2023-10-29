using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Trigger,
        Collision,
    }
    public TriggerType triggerType;

    void OnTriggerEnter(Collider other)
    {
        if (triggerType == TriggerType.Trigger && other.CompareTag("Player"))
        {
            Triggered();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (triggerType == TriggerType.Collision && collision.gameObject.CompareTag("Player"))
        {
            Triggered();
        }
    }

    void Triggered()
    {
        GameManager gameManager = GameManager.instance;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (gameManager.gameState == GameState.Over) return;
        
        // Hide and freeze the player
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        // Play explosion particle
        gameManager.explosionParticle.Play();
        // Game over
        gameManager.GameOver();
    }
}
