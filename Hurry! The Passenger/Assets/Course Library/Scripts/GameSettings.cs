using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This script is used to share between scene to reflect on game setting set on the title screen
public class GameSettings : ScriptableObject
{
    // Default sensitivity
    public int sensitivity = 200;

    private static GameSettings instance_;
    public static GameSettings instance
    {
        get
        {
            if (instance_ == null)
            {
                // Create an instance with default values on first access
                instance_ = ScriptableObject.CreateInstance<GameSettings>();
            }
            return instance_;
        }
    }

    #if UNITY_EDITOR
    [MenuItem("Game Settings/View Game Settings", false, 0)]
    public static void ViewGameSettings()
    {
        Selection.activeObject = instance;
    }
    #endif
}
