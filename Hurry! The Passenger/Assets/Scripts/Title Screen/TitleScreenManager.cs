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
        continueButton.SetActive(PlayerPrefs.HasKey("ContinueSceneName"));

        // Update sensitivity text
        enterSensitivity.text = GameSettings.instance.sensitivity.ToString();
    }

    // Game Start
    public void StartGame(bool continueGame)
    {
        // Clear continue scene if we are not continuing
        if (continueGame)
        {
            // PlayerPrefs.DeleteKey("ContinueSceneName");
            PlayerPrefs.SetInt("ContinueOrNot", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ContinueOrNot", 0);
        }

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
        GameSettings.Delete();
        PlayerPrefs.DeleteAll();
        RefreshTitleScreen();
    }
}
