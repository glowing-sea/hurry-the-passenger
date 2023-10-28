using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is used to store the functions associate with the button and input text on the title screen
public class TitleScreenManager : MonoBehaviour
{
    public TMPro.TMP_InputField enterSensitivity;
    public GameObject continueButton;
    public GameObject settingMenu;

    void Start()
    {
        enterSensitivity.onEndEdit.AddListener((sensitivity) => SetSensitivity(float.Parse(sensitivity)));
        RefreshTitleScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Toggle setting menu
            settingMenu.SetActive(!settingMenu.activeSelf);
        }
    }

    // Refresh title screen element states to reflect on game settings and save
    private void RefreshTitleScreen()
    {
        // Hide Continue Button if no save data
        continueButton.SetActive(GameProgress.Exists());

        // Update sensitivity text
        enterSensitivity.text = GameSettings.instance.sensitivity.ToString();
    }

    // Erase save to start a new game
    public void NewGame()
    {
        if (GameProgress.Exists())
        {
            ConfirmationDialog.Show("Start the game from the beggining?\n(previous progress will be lost)", () => 
            {
                GameProgress.Delete();
                StartGame();
            });
        }
        else
        {
            GameProgress.Delete();
            StartGame();
        }
    }

    // Start or continue the game
    public void StartGame()
    {
        // Load the game sence
        SceneManager.LoadScene("Common");
    }

    // Quit the game
    public void QuiteGame()
    {
        Application.Quit();
    }

    void SetSensitivity(float sensitivity)
    {
        GameSettings.instance.sensitivity = sensitivity;
        GameSettings.instance.Save();
    }

    public void DeleteAllSave()
    {
        GameProgress.Delete();
        GameSettings.Delete();
        RefreshTitleScreen();
    }
}
