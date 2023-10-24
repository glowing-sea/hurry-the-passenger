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

    // Hide Continue Button if no save data
    void Start()
    {
        // Disable self if there is no continue scene
        if (!PlayerPrefs.HasKey("ContinueSceneName"))
        {
            continueButton.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (settingMenu.activeSelf)
            {
                settingMenu.SetActive(false);
            } else
            {
                settingMenu.SetActive(true);
            }
        }
    }

    // Game Start
    public void StartGame(bool continueGame)
    {
        try
        {
            // Change player control sensitivity
            GameSettings.instance.sensitivity = float.Parse(enterSensitivity.text);
            GameSettings.instance.Save();
            Debug.Log("Sensitivity set to " + GameSettings.instance.sensitivity);
        }
        catch
        {
        }

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

    public void OpenSettings()
    {
        settingMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        settingMenu.SetActive(false);
        if (!PlayerPrefs.HasKey("ContinueSceneName"))
        {
            continueButton.SetActive(false);
        }
    }

    public void DeleteAllSave()
    {
        PlayerPrefs.DeleteAll();
        enterSensitivity.text = "0.1";
    }
}
