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
    public float timeRemainMinute;
    public int timeRemain;
    public float gravity;

    // Game state
    private GameState gameState_;

        public bool passedTut;
    public GameState gameState
    {
        get { return gameState_; }
        set
        {
            gameState_ = value;

            // Control mouse lock and visibility
            switch (gameState_)
            {
                case GameState.Running:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                default:
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }
    }


    // UI
    public TextMeshProUGUI timeRemainText;


    // Menus
    public GameObject taskMenu;
    public GameObject guideMenu;
    public GameObject pauseMenu;
    public TMPro.TMP_InputField checkMenu;


    public TextMeshProUGUI noteText; // show guides when player press I
    public Slider staminaGauge;

    public TextMeshProUGUI smallText;
    public TextMeshProUGUI largeText;

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
    void Start() {
        // Get the reference to the player
        player = GameObject.Find("Player");
                passedTut = false;

        // Whether change player's spawning point (mainly for testing)
        if (DebugSettings.instance.changeSpawningPoint)
        {
            player.SetActive(false);
            player.transform.position = DebugSettings.instance.newSpawningPoint;
            player.transform.eulerAngles = DebugSettings.instance.newSpawningRotation;
            player.SetActive(true);
        }

        // Whether to pop up a guide when started
        if (DebugSettings.instance.showGuideWhenStart)
        {
            gameState = GameState.Paused;
            guideMenu.SetActive(true);
        } else
        {
            gameState = GameState.Running;
        }
        
        // make the mouse inavtive for 0.2 seconds to wait for the game to be fully loaded
        // StartCoroutine(WaitForLoading());
        Time.timeScale = 1;
        timeRemain = (int) (timeRemainMinute * 60); // convert minute to second
        timeRemainText.text = displayTime(timeRemain); // display time remain
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
        // Press some key to open and close menu
        var menus = new (KeyCode, GameObject)[] {
            (KeyCode.T, taskMenu),
            (KeyCode.G, guideMenu),
            (KeyCode.P, pauseMenu),
            (KeyCode.BackQuote, checkMenu.gameObject),
        };

        foreach (var (key, menu) in menus)
        {
            if (Input.GetKeyDown(key))
            {
                if (gameState == GameState.Running)
                {
                    gameState = GameState.Paused;
                    menu.SetActive(true);
                }
                else if (gameState == GameState.Paused && menu.activeSelf == true)
                {
                    gameState = GameState.Running;
                    menu.SetActive(false);
                }
            }
        }
    }

    // Close a task menu / tutorial menu / pause menu
    public void CloseMenu()
    {
        gameState = GameState.Running;
        taskMenu.SetActive(false);
        guideMenu.SetActive(false);
        pauseMenu.SetActive(false);
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
        gameState = GameState.Running;
    }

    // Count Down Time Remain 
    public IEnumerator TimeRemain()
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
            if (gameState == GameState.Running || gameState == GameState.LeavingMainScene){
                timeRemain--;
            }
        }
    }

    // What to do when the player fails
    public void GameOver()
    {
        gameState = GameState.Over;
        largeText.text = "Game Over";
        largeText.gameObject.SetActive(true);
        pauseMenu.SetActive(true);
        playerAudio.PlayOneShot(crashSound, 1.0f); // play crash sound
    }

    // What to do when the player wins
    public void GameFinished()
    {
        gameState = GameState.Over;
        largeText.text = "Game Finished";
        largeText.gameObject.SetActive(true);
        pauseMenu.SetActive(true);
        playerAudio.PlayOneShot(taskComplete, 1.0f); // play crash sound
    }

    //Continue Game State After Paused
    public void ContinueGame()
    {
        gameState = GameState.Running;
        pauseMenu.SetActive(false);
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
        string content = "Hurry! You are almost late!\nYou only have *10 minutes* left to approach *International Departure* and \nto catch the plane, you need to:\n\n";
        string task1 = tasks[0] ? "[Completed] " : "[Unfinished] ";
        string task2 = tasks[1] ? "[Completed] " : "[Unfinished] ";
        string task3 = tasks[2] ? "[Completed] " : "[Unfinished] "; 
        task1 = task1 + "Cross roads to the airport\n";
        task2 = task2 + "Organise your baggage to avoid being overweight\n";
        task3 = task3 + "Find the right check-in counter and departure gate\n";
        string note = title + content + task1 + task2 + task3;
        noteText.text = note;
    }


    // Behaviour of the command line
    public void InputCommandConfirm()
    {
        switch (checkMenu.text)
        {
            case "unlimitedtime":
                timeRemain = 5940;
                break;
            case "skipbarrier1":
            case "":
                player.SetActive(false);
                player.transform.position = new Vector3(27, 0, 0);
                player.SetActive(true);
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
