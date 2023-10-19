using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// This script is used to store the functions associate with the button and input text on the title screen
public class ButtonActions : MonoBehaviour
{
    public TMPro.TMP_InputField enterSensitivity;
    public GameObject continueButton;

    // Hide Continue Button if no save data
    void Start()
    {
        // Disable self if there is no continue scene
        if (!PlayerPrefs.HasKey("ContinueSceneName"))
        {
            continueButton.SetActive(false);
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
        } else
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
}
