using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    // Manage tasks the player has finished
    // Only when all tasks is finished, the player can go to the international depature
    public bool[] tasks;

    // Internal Variables
    // -1 Over, 0 Playing, 1 Pause by the system, 2 Pause by Esc, 3 Pause by [I], 4 Organise baggage
    public int gameState;
    public float timeRemainMinute;
    public int timeRemain;
    public float gravity;


    // UI
    public GameObject guides; // show guides when player press I
    public TextMeshProUGUI timeRemainText;
    public Button restartButton;
    public Button titleScreenButton;
    public GameObject notes;
    public TextMeshProUGUI noteText; // show guides when player press I
    public Slider staminaGauge;

    public TextMeshProUGUI smallText;
    public TextMeshProUGUI largeText;

    public TMPro.TMP_InputField inputCommand; // input cheat command

    // Cameras
    public Camera playerCamera;
    public Camera baggageCamera;


    // Sound effect
    private AudioSource playerAudio;
    public AudioClip crashSound;
    public AudioSource roadAmbiance;
    public AudioSource airportAmbiance;

    // Bgm
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip taskComplete;
    public AudioClip somethingWrong;
    public AudioSource bgm;
    public AudioSource soundEffect;
    private float bgmVolumn = 0.2f;


    // Particle
    public ParticleSystem explosionParticle;


    // Script
    private PlayerController playerController; // reference to the game manager script

    // Player
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the player
        player = GameObject.Find("Player");

        // make the mouse inavtive for 0.2 seconds to wait for the game to be fully loaded
        StartCoroutine(WaitForLoading());
        Cursor.visible = false;
        Time.timeScale = 1;
        timeRemain = (int) (timeRemainMinute * 60); // convert minute to second
        timeRemainText.text = displayTime(timeRemain); // display time remain
        StartCoroutine(ShowThingTemporarily(guides, 3)); // hide control guides 3 second later
        StartCoroutine(TimeRemain()); // set up time countdown
        playerAudio = GetComponent<AudioSource>(); // get audio playing reference
        Physics.gravity = new Vector3(0, -gravity, 0); // set gravity
        UpdateNotesMenu(); // update the note menu (when the player press [I])

        // Set up bgm
        bgm = GameObject.Find("Player Camera").GetComponent<AudioSource>(); // get bgm audio source
        bgm.loop = true;
        bgm.clip = bgm1;
        bgm.volume = bgmVolumn;
        bgm.Play();

        // Set up soundEffect player
        soundEffect = player.GetComponent<AudioSource>(); // get bgm audio source

        // Set up ambiance sound effect
        roadAmbiance = GameObject.Find("Road Ambiance").GetComponent<AudioSource>();
        airportAmbiance = GameObject.Find("Airport Ambiance").GetComponent<AudioSource>();
        roadAmbiance.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Player press I to pause the game and see notes
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gameState == 0)
            {
                gameState = 3; // Puase by I
                notes.SetActive(true);
                Cursor.visible = true;
            } else if (gameState == 3)
            {
                gameState = 0;
                notes.SetActive(false);
                Cursor.visible = false;
            }
        }

        // Player press Esc to pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == 0)
            {
                gameState = 2; // Puase by Esc
                titleScreenButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                Cursor.visible = true;
            }
            else if (gameState == 2)
            {
                gameState = 0;
                titleScreenButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
                Cursor.visible = false;
            }
        }

        // Player press BackQuote to pause the game
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (gameState == 0)
            {
                gameState = 2; // Puase by BackQuote
                inputCommand.gameObject.SetActive(true);
                Cursor.visible = true;
            }
            else if (gameState == 2)
            {
                gameState = 0;
                inputCommand.gameObject.SetActive(false);
                Cursor.visible = false;
            }
        }
    }


    // Show someting on the canvas temporarily
    public IEnumerator ShowThingTemporarily(GameObject gameObject, float time)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }


    // Hide control guides after 3 second
    IEnumerator WaitForLoading()
    {
        yield return new WaitForSeconds(0.2f);
        gameState = 0;
    }

    // Count Down Time Remain 
    IEnumerator TimeRemain()
    {
        while (true)
        {
            if (timeRemain < 0)
            {
                GameOver();
                break;
            }
            timeRemainText.text = displayTime(timeRemain);
            yield return new WaitForSeconds(1);
            if (gameState == 0 || gameState == 4){
                timeRemain--;
            }
        }
    }

    // What to do when the player fails
    public void GameOver()
    {
        gameState = -1;
        largeText.text = "Game Over";
        largeText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        titleScreenButton.gameObject.SetActive(true);
        playerAudio.PlayOneShot(crashSound, 1.0f); // play crash sound
        Cursor.visible = true;
    }

    // What to do when the player wins
    public void GameFinished()
    {
        gameState = -1;
        largeText.text = "Game Finished";
        largeText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        titleScreenButton.gameObject.SetActive(true);
        playerAudio.PlayOneShot(taskComplete, 1.0f); // play crash sound
        Cursor.visible = true;
    }


    // Reload the Barrier 1 & 2
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Back to title screen
    public void ToTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }


    // Display minute : second given how many second left
    public string displayTime(int timeRemain)
    {
        if (timeRemain > 0)
        {
            int second = timeRemain % 60;
            if (second < 10)
            {
                return (timeRemain / 60) + ":0" + second;
            }
            else
            {
                return (timeRemain / 60) + ":" + second;
            }
        } else
        {
            return "0:00";
        }

    }

    // Update Notes menu text. (Note record what tasks has been completed or incompleted)
    public void UpdateNotesMenu()
    {
        string title = "Notes\n\n";
        string content = "Welcome to this short demo. You are now a person who almost late for the plane. You have to get to the “International Departure” in 5 minutes, but before this, you should:\n\n";
        string task1 = tasks[0] ? "[Completed] " : "[Unfinished] ";
        string task2 = tasks[1] ? "[Completed] " : "[Unfinished] ";
        string task3 = tasks[2] ? "[Completed] " : "[Unfinished] ";
        task1 = task1 + "Cross roads to the airport\n";
        task2 = task2 + "Organise your baggage and avoid being overweight\n";
        task3 = task3 + "Find the right check-in counter to check in\n";
        string note = title + content + task1 + task2 + task3;
        noteText.text = note;
    }


    // Behaviour of the command line
    public void InputCommandConfirm()
    {
        switch (inputCommand.text)
        {
            case "unlimitedtime":
                timeRemain = 5940;
                break;
            case "skipbarrier1":
                player.transform.position = new Vector3(27, 0, 0);
                break;
            case "skipbarrier2":
                tasks[1] = true;
                break;
            case "skipbarrier3":
                tasks[2] = true;
                break;
        }
    }
}
