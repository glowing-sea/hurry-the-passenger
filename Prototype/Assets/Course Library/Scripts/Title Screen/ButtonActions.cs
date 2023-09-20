using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// This script is used to store the functions associate with the button and input text on the title screen
public class ButtonActions : MonoBehaviour
{
    public TMPro.TMP_InputField enterSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Game Start
    public void StartGame()
    {
        try
        {
            // Change player control sensitivity
            GameSettings.sensitivity = int.Parse(enterSensitivity.text);
            Debug.Log(int.Parse(enterSensitivity.text));
        }
        catch
        {
        }

        // Load the game sence
        SceneManager.LoadScene("Barrier 1 & 2");
    }

    // Quit the game
    public void QuiteGame()
    {
        Application.Quit();
    }
}
