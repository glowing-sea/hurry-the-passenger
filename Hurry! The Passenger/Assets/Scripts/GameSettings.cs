using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

// This script is used to share between scene to reflect on game setting set on the title screen
public class GameSettings : ScriptableObject
{
    // Mouse sensitivity, in degrees per pixel
    public float sensitivity = 0.1f;//adjust sensitivity from 1 to 0.1 to improve player's experience

    private static GameSettings instance_;
    public static GameSettings instance
    {
        get
        {
            if (instance_ == null)
            {
                // Load from PlayerPrefs
                instance_ = ScriptableObject.CreateInstance<GameSettings>();
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("GameSettings", "{}"), instance_);
            }
            return instance_;
        }
    }

    public void Save()
    {
        // Save to PlayerPrefs
        PlayerPrefs.SetString("GameSettings", JsonUtility.ToJson(this));
    }

    public static void Delete()
    {
        instance_ = null;
        PlayerPrefs.DeleteKey("GameSettings");
    }

    #if UNITY_EDITOR

    [MenuItem("Game Settings/View Game Settings", false, 0)]
    public static void ViewGameSettings()
    {
        Selection.activeObject = instance;
    }

    [MenuItem("Game Settings/Reset Game Settings", false, 100)]
    public static void ResetGameSettings()
    {
        // Confirm dialog
        if (!EditorUtility.DisplayDialog("Reset Game Settings", "Are you sure you want to reset game settings?", "Yes", "No")) return;

        Delete();
        Selection.activeObject = null;
    }

    [CustomEditor(typeof(GameSettings))]
    public class GameSettingsEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var settings = (GameSettings) target;
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // Add save button
            var saveButton = new Button(() =>
            {
                settings.Save();
                Debug.Log("Game settings saved");
            });
            saveButton.text = "Save";
            root.Add(saveButton);

            return root;
        }
    }

    #endif
}
