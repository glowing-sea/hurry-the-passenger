using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleContinueButton : MonoBehaviour
{
    void Start()
    {
        // Disable self if there is no continue scene
        if (PlayerPrefs.GetString("ContinueSceneName", "") == "")
        {
            gameObject.SetActive(false);
        }
    }
}
