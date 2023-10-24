using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a zone in which some audio can be played
/// </summary>
[RequireComponent(typeof(Collider))]
public class AudioZone : MonoBehaviour
{
    public AudioClip bgm;
    public AudioClip ambience;

    private AudioSource bgmPlayer;
    private AudioSource ambiencePlayer;

    void Start()
    {
        // Get the reference to the players
        bgmPlayer = GameManager.instance.bgmPlayer;
        ambiencePlayer = GameManager.instance.ambiencePlayer;

        // Make sure the collider is a trigger
        if (GetComponent<Collider>().isTrigger == false)
        {
            Debug.LogWarning("AudioZone: Collider is not a trigger. Audio will not play.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enter();
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Exit();
        }
    }

    void Enter()
    {
        Debug.Log($"Entered audio zone {gameObject.name}");

        // Play BGM on a player-attached audio source
        if (bgm != null && bgmPlayer.clip != bgm)
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = bgm;
            bgmPlayer.Play();
        }

        // Play ambience on an audio source following the player
        if (ambience != null && ambiencePlayer.clip != ambience)
        {
            ambiencePlayer.Stop();
            ambiencePlayer.clip = ambience;
            ambiencePlayer.Play();
        }
    }

    void Exit()
    {
        Debug.Log($"Exited audio zone {gameObject.name}");

        // Stop BGM
        if (bgm != null && bgmPlayer.clip == bgm)
        {
            bgmPlayer.Stop();
            bgmPlayer.clip = null;
        }

        // Stop ambience
        if (ambience != null && ambiencePlayer.clip == ambience)
        {
            ambiencePlayer.Stop();
            ambiencePlayer.clip = null;
        }
    }
}
