using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Slider = UnityEngine.UI.Slider;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

public class GameManager : MonoBehaviour
{
    // Manage tasks the player has finished
    private Dictionary<PlayerTask, PlayerTask.State> taskStates = new();

    public int initialTime;
    [System.NonSerialized] public int timeRemain;
    [System.NonSerialized] public bool timerEnabled;

    public int balance;

    public float gravity;

    // Game state
    private GameState gameState_;
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
    public TextMeshProUGUI balanceText;

    // Menus
    public GameObject taskMenu;
    public GameObject guideMenu;
    public GameObject pauseMenu;
    public TMPro.TMP_InputField cheatMenu;

    // Exclamation Mark to notify the player there is an update in GuidMenu or TaskMenu
    [SerializeField] private GameObject guideMenuUpdateMark;
    [SerializeField] private GameObject taskMenuUpdateMark;

    public TextMeshProUGUI noteText; // show guides when player press I
    public Slider staminaGauge;

    public TextMeshProUGUI smallText;
    public TextMeshProUGUI largeText;
    public TextMeshProUGUI text;

    // Cameras
    public Camera playerCamera;


    // Sound effect
    public AudioClip crashSound;
    public AudioClip taskComplete;
    public AudioClip somethingWrong;
    public AudioClip pickUpMoney;

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

        // Create state for all tasks
        List<PlayerTask> tasks = Resources.LoadAll<PlayerTask>("Player Tasks").ToList();
        tasks.Sort();
        foreach (var task in tasks)
        {
            taskStates.Add(task, new PlayerTask.State());
        }

        // Load a new state or a continue state
        GameProgress savedProgress = GameProgress.Load();
        if (savedProgress == null)
        {
            currentSceneName = startingSceneName;
            timeRemain = initialTime;
        }
        else
        {
            currentSceneName = savedProgress.sceneName;
            timeRemain = savedProgress.timeRemain;
            balance = savedProgress.balance;
        }

        // In the editor, if another scene is already load, we set the continue scene to that.
        // This prevents the continue scene from last time from being loaded when
        // we are developing on another scene.
        #if UNITY_EDITOR
        if (SceneManager.sceneCount > 1)
        {
            if (SceneManager.GetSceneAt(1).name != currentSceneName)
            {
                ReachNewBarrier(SceneManager.GetSceneAt(1).name);
            }
        }
        #endif

        // Load current scene
        LoadScene(currentSceneName, () =>
        {
            // Move player to spawn point
            SpawnPoint spawnPoint = SpawnPoint.FindInScene(currentSceneName);
            TeleportPlayer(spawnPoint.transform.position, spawnPoint.transform.rotation);

            // Override spawning point with DebugSettings
            if (DebugSettings.instance.changeSpawningPoint)
            {
                TeleportPlayer(DebugSettings.instance.newSpawningPoint, DebugSettings.instance.newSpawningRotation);
            }
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


        StartCoroutine(TimerUpdate());
        // Enable timer as long as we are not in the starting scene,
        // for which the timer is started by finishing the tutorial.
        if (currentSceneName != startingSceneName) timerEnabled = true;

        Physics.gravity = new Vector3(0, -gravity, 0); // set gravity
        UpdateTaskMenu(); // update the note menu (when the player press [I])
        UpdateBalance(0, true); // update balance on the UI

    }

    // Update is called once per frame
    void Update()
    {

        
        // Press some key to open and close menu
        var menus = new (KeyCode, GameObject)[] {
            (KeyCode.T, taskMenu),
            (KeyCode.G, guideMenu),
            (KeyCode.P, pauseMenu),
            (KeyCode.BackQuote, cheatMenu.gameObject),
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
                    CloseMenu(menu.name);
                }
            }
        }
    }

    // Close a task menu / tutorial menu / pause menu
    public void CloseMenu(string menuName)
    {
        gameState = GameState.Running;
        taskMenu.SetActive(false);
        guideMenu.SetActive(false);
        pauseMenu.SetActive(false);
        cheatMenu.gameObject.SetActive(false);

        if (menuName == "Guide Menu")
            guideMenuUpdateMark.SetActive(false);
        if (menuName == "Task Menu")
            taskMenuUpdateMark.SetActive(false);
    }

    public void ShowThingTemporarily(GameObject gameObject, float time)
    {
        StartCoroutine(ShowThingTemporarilyEnumerator(gameObject, time));
    }

    public void ShowThingTemporarily(TextMeshProUGUI text, float time)
    {
        StartCoroutine(ShowThingTemporarilyEnumerator(text.gameObject, time));
    }

    // Show someting on the canvas temporarily
    private IEnumerator ShowThingTemporarilyEnumerator(GameObject gameObject, float time)
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

    // Countdown timer
    private IEnumerator TimerUpdate()
    {
        while (true)
        {
            if (timeRemain < 0)
            {
                GameOver();
                break;
            }

            TimeUpdateImmediately();

            yield return new WaitForSeconds(1);

            bool isRunning = gameState == GameState.Running || gameState == GameState.LeavingMainScene;
            if (timerEnabled && isRunning)
            {
                timeRemain--;
            }
        }
    }

    private void TimeUpdateImmediately()
    {
        // Display minute : second given how many second left
        if (timerEnabled)
        {
            int minute = System.Math.Max(timeRemain / 60, 0);
            int second = System.Math.Max(timeRemain % 60, 0);
            timeRemainText.text = string.Format("{0:d2}:{1:d2}", minute, second);
        }
        else
        {
            timeRemainText.text = "";
        }
    }

    public void TimeDecrese(int time)
    {
        timeRemain -= time;
        TimeUpdateImmediately();
    }

    // What to do when the player fails
    public bool isGameOver = false; 

    public void GameOver()
    {
        gameState = GameState.Over;
        largeText.text = "Game Over";
        largeText.gameObject.SetActive(true);
        pauseMenu.SetActive(true);
        sfxPlayer.PlayOneShot(crashSound, 1.0f); // play crash sound
        isGameOver = true; 

    }

    // What to do when the player wins
    public void GameFinished()
    {
        gameState = GameState.Over;
        largeText.text = "Game Finished";
        largeText.gameObject.SetActive(true);
        pauseMenu.SetActive(true);
        sfxPlayer.PlayOneShot(taskComplete, 1.0f); // play crash sound
        isGameOver = true; 
    }

    // Continue Game State After Paused
    public void ContinueGame()
    {
        if(isGameOver)
        {
            Debug.Log("Game Over. Can't continue.");
            return; // Can't continue if game is over
        }
        gameState = GameState.Running;
        pauseMenu.SetActive(false);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }

    // Erase save to start a new game
    public void NewGame()
    {
        ConfirmationDialog.Show("Start the game from the beggining?\n(previous progress will be lost)", () => 
        {
            GameProgress.Delete();
            ReloadGame();
        });
    }

    // Back to title screen
    public void ToTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }

    // Update Notes menu text. (Note record what tasks has been completed or incompleted)
    public void UpdateTaskMenu()
    {
        StringBuilder sb = new();
        sb.Append("Notes\n\n");
        sb.Append("Hurry! You are almost late!\nYou only have *15 minutes* left to board a *Domestic Flight* to Sydney, \nnow you need to:\n\n");
        foreach (var (task, state) in taskStates)
        {
            // Display only tasks in the current scene and that are visible
            if (task.sceneName == currentSceneName && task.isVisible)
            {
                sb.Append(state.isComplete ? "[Completed] " : "[Unfinished] ");
                sb.Append(task.taskName);
                sb.Append("\n");
            }
        }
        noteText.text = sb.ToString();
        taskMenuUpdateMark.SetActive(true);
    }


    // Behaviour of the command line
    public void InputCommandConfirm()
    {
        string command = cheatMenu.text;

        if (command.StartsWith("skip "))
        {
            command = command.Substring(5);
            CompleteTask(command);
        }
        if (command.StartsWith("ContinueSceneName = "))
        {
            command = command.Substring(20);
            if (!SceneManager.GetSceneByName(command).IsValid())
            {
                Debug.LogError("Scene " + command + " does not exist");
                return;
            }

            var progress = GameProgress.Load() ?? GameProgress.Create();
            progress.sceneName = command;
            progress.timeRemain = 600;
            GameProgress.Save(progress);

            sfxPlayer.PlayOneShot(taskComplete, 1.0f);
        }

        switch (cheatMenu.text)
        {
            case "unlimitedtime":
                timeRemain = 5940;
                break;
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


    // Increase or decrease the money. If negative, return false
    public bool UpdateBalance(int amount, bool increase)
    {
        // Play sound if the balance increase
        if (amount != 0)
        {
            sfxPlayer.PlayOneShot(pickUpMoney, 2);
        }

        int newBalance = balance = increase ? balance + amount : balance - amount;
        if (newBalance >= 0)
        {
            balance = newBalance;
            balanceText.text = "$ " + balance;
            return true;
        }
        else
        {
            return false;
        }

    }


    /// <summary>
    /// Called when player reaches a new barrier.<br>
    /// Save the game progress and update the TaskMenu
    /// </summary>
    public void ReachNewBarrier(string sceneName)
    {
        currentSceneName = sceneName;

        var newProgress = GameProgress.Create();
        newProgress.sceneName = sceneName;
        newProgress.timeRemain = timeRemain;
        newProgress.balance = balance;
        GameProgress.Save(newProgress);

        ShowThingTemporarily(mainUI.autoSavingIndicator, 2);
        Debug.Log("Checkpoint: " + sceneName);

        // Scene has changed, so different tasks will display
        UpdateTaskMenu();
    }



    public PlayerTask.State GetTaskState(PlayerTask task)
    {
        return taskStates[task];
    }

    // Mark a task as completed given the task object
    public void CompleteTask(PlayerTask task)
    {
        var state = taskStates[task];
        state.isComplete = true;
        taskStates[task] = state;
        sfxPlayer.PlayOneShot(taskComplete, 1.0f);
        UpdateTaskMenu();
    }

    // Mark a task as completed given the task ID
    public void CompleteTask(string taskID)
    {
        PlayerTask task = null;
        foreach (KeyValuePair<PlayerTask, PlayerTask.State> entry in taskStates)
        {
            if (entry.Key.taskID == taskID)
                task = entry.Key;
        }
        if (task != null)
            CompleteTask(task);
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        ListView taskListView;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var gameManager = (GameManager) target;
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            if (Application.isPlaying)
            {

                // Add task list
                var Label = new Label("Tasks")
                {
                    style =
                    {
                        marginTop = 10,
                        marginBottom = 5,
                        unityFontStyleAndWeight = FontStyle.Bold,
                    }
                };
                root.Add(Label);
                taskListView = new ListView(gameManager.taskStates.Keys.ToList(), -1, () =>
                {
                    var element = new VisualElement()
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Row,
                            alignItems = Align.Center,
                        }
                    };

                    var checkbox = new UnityEngine.UIElements.Toggle();
                    checkbox.RegisterValueChangedCallback((ChangeEvent<bool> evt) =>
                    {
                        var task = (PlayerTask) element.userData;
                        var state = gameManager.taskStates[task];
                        state.isComplete = evt.newValue;
                        gameManager.taskStates[task] = state;
                        gameManager.UpdateTaskMenu();
                    });

                    var label = new Label()
                    {
                        style =
                        {
                            flexGrow = 1,
                        }
                    };

                    element.Add(label);
                    element.Add(checkbox);
                    return element;

                }, (VisualElement element, int index) =>
                {
                    var (task, state) = gameManager.taskStates.ElementAt(index);
                    element.userData = task;
                    var label = element.Q<Label>();
                    label.text = task.taskName;
                    var checkbox = element.Q<UnityEngine.UIElements.Toggle>();
                    checkbox.value = state.isComplete;
                });

                taskListView.selectionType = SelectionType.None;
                taskListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                root.Add(taskListView);
            }

            return root;
        }

        void Update()
        {
            if (Application.isPlaying)
            {
                taskListView?.RefreshItems();
            }
        }

        void OnEnable() { EditorApplication.update += Update; }
        void OnDisable() { EditorApplication.update -= Update; }
    }
    #endif
}
