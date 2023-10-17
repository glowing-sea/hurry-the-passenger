using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class GameManager : MonoBehaviour
{
    // Manage tasks the player has finished
    public List<PlayerTask.State> tasks { get; private set; } = new List<PlayerTask.State>();

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

    [SerializeField] private string startingSceneName;
    public string currentSceneName { get; private set; }


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


    // Sound effect
    public AudioClip crashSound;
    public AudioClip taskComplete;
    public AudioClip somethingWrong;

    public AudioSource bgmPlayer;
    public AudioSource ambiencePlayer;
    public AudioSource sfxPlayer;


    // Particle
    public ParticleSystem explosionParticle;

    public MainUI mainUI;
    public DialogSystem dialogSystem;


    // Script
    private PlayerController playerController; // reference to the game manager script

    // Player
    private GameObject player;

    // Helper property to get the instance of the game manager
    public static GameManager instance
    {
        get
        {
            GameObject obj = GameObject.FindGameObjectWithTag("GameController");
            if (obj != null)
            {
                return obj.GetComponent<GameManager>();
            } else {
                throw new System.NullReferenceException("No game manager loaded. Did you load the scene that contains the game manager?");
            }
        }
    }


    // Start is called before the first frame update
    void Start() {
        // Get the reference to the player
        player = GameObject.Find("Player");
        passedTut = false;

        // Start from continued scene
        currentSceneName = PlayerPrefs.GetString("ContinueSceneName", startingSceneName);

        // In the editor, if another scene is already load, we set the continue scene to that.
        // This prevents the continue scene from last time from being loaded when
        // we are developing on another scene.
        #if UNITY_EDITOR
        if (SceneManager.sceneCount > 1)
        {
            if (SceneManager.GetSceneAt(1).name != currentSceneName)
            {
                ReachSceneCheckPoint(SceneManager.GetSceneAt(1).name);
            }
        }
        #endif

        // Load current scene
        LoadScene(currentSceneName, () =>
        {
            // Move player to spawn point
            CheckPoint spawnPoint = CheckPoint.FindInScene(currentSceneName);
            TeleportPlayer(spawnPoint.transform.position, spawnPoint.transform.rotation);

            // Override spawning point with DebugSettings
            if (DebugSettings.instance.changeSpawningPoint)
            {
                TeleportPlayer(DebugSettings.instance.newSpawningPoint, DebugSettings.instance.newSpawningRotation);
            }

            // Load tasks
            ReloadTasksFromCheckpoint(spawnPoint);
        });


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
        Physics.gravity = new Vector3(0, -gravity, 0); // set gravity
        UpdateNotesMenu(); // update the note menu (when the player press [I])

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
        sfxPlayer.PlayOneShot(crashSound, 1.0f); // play crash sound
    }

    // What to do when the player wins
    public void GameFinished()
    {
        gameState = GameState.Over;
        largeText.text = "Game Finished";
        largeText.gameObject.SetActive(true);
        pauseMenu.SetActive(true);
        sfxPlayer.PlayOneShot(taskComplete, 1.0f); // play crash sound
    }

    //Continue Game State After Paused
    public void ContinueGame()
    {
        gameState = GameState.Running;
        pauseMenu.SetActive(false);
    }

    // Restart Game
    public void RestartGame()
    {
        SceneManager.LoadScene(gameObject.scene.name);
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
        StringBuilder sb = new();
        sb.Append("Notes\n\n");
        sb.Append("Hurry! You are almost late!\nYou only have *10 minutes* left to approach *International Departure* and \nto catch the plane, you need to:\n\n");
        foreach (PlayerTask.State task in tasks)
        {
            if (task.task.isVisible)
            {
                sb.Append(task.isComplete ? "[Completed] " : "[Unfinished] ");
                sb.Append(task.task.taskName);
                sb.Append("\n");
            }
        }
        noteText.text = sb.ToString();
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
        }
    }

    public void TeleportPlayer(Vector3 position, Quaternion? rotation)
    {
        player.SetActive(false);
        player.transform.position = position;
        if (rotation != null)
        {
            player.transform.rotation = rotation.Value;
        }
        player.SetActive(true);
    }
    public void TeleportPlayer(Vector3 position, Vector3 eulerAngles)
    {
        TeleportPlayer(position, Quaternion.Euler(eulerAngles));
    }

    public void LoadScene(string sceneName, System.Action callback = null)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            op.completed += (AsyncOperation obj) =>
            {
                callback?.Invoke();
            };
        } else {
            callback?.Invoke();
        }
    }

    public void UnloadScene(string sceneName, System.Action callback = null)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            op.completed += (AsyncOperation obj) =>
            {
                callback?.Invoke();
            };
        } else {
            callback?.Invoke();
        }
    }

    // Discard tasks from the previous scene and load tasks from the new scene.
    private void ReloadTasksFromCheckpoint(CheckPoint checkPoint)
    {
        tasks.Clear();
        tasks.AddRange(checkPoint.tasks.Select((PlayerTask task) => task.CreateState()));
        UpdateNotesMenu();
    }

    /// <summary>
    /// Called when player reaches a scene's checkpoint trigger.<br>
    /// Set the scene the game will continue from when the game is restarted, and reload tasks.
    /// </summary>
    public void ReachSceneCheckPoint(string sceneName)
    {
        currentSceneName = sceneName;
        PlayerPrefs.SetString("ContinueSceneName", sceneName);
        PlayerPrefs.Save();

        Debug.Log("Checkpoint: " + sceneName);

        ReloadTasksFromCheckpoint(CheckPoint.FindInScene(sceneName));
    }



    public PlayerTask.State GetTaskState(PlayerTask task)
    {
        int index = tasks.FindIndex((PlayerTask.State state) => state.task == task);
        if (index == -1)
        {
            throw new System.ArgumentException("Task not loaded: " + task.taskName);
        }
        return tasks[index];
    }

    public void CompleteTask(int index)
    {
        PlayerTask.State task = tasks[index];
        if (!task.isComplete)
        {
            task.isComplete = true;
            UpdateNotesMenu();
            sfxPlayer.PlayOneShot(taskComplete, 1.0f);
            Debug.Log("Task complete: " + task.task.taskName);
        }
    }
    public void CompleteTask(PlayerTask task)
    {
        int index = tasks.FindIndex((PlayerTask.State state) => state.task == task);
        if (index == -1)
        {
            throw new System.ArgumentException("Task not loaded: " + task.taskName);
        }
        CompleteTask(index);
    }
}
