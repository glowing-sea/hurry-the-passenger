using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

// A record of the progress of the game, stored in PlayerPrefs
public class GameProgress : ScriptableObject
{
    public string sceneName;
    public int timeRemain;
    public int balance;

    private static readonly string playerPrefsKey = "GameProgress";

    public static GameProgress Create()
    {
        var instance = ScriptableObject.CreateInstance<GameProgress>();
        return instance;
    }

    public static bool Exists()
    {
        return PlayerPrefs.HasKey(playerPrefsKey);
    }

    public static GameProgress Load()
    {
        if (!Exists()) return null;

        // Load from PlayerPrefs
        var instance = ScriptableObject.CreateInstance<GameProgress>();
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(playerPrefsKey), instance);
        return instance;
    }

    public static void Save(GameProgress instance)
    {
        // Save to PlayerPrefs
        PlayerPrefs.SetString(playerPrefsKey, JsonUtility.ToJson(instance));
        PlayerPrefs.Save();
    }

    public static void Delete()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
        PlayerPrefs.Save();
    }

    #if UNITY_EDITOR

    [MenuItem("Game Save/View Game Progress", false, 0)]
    public static void ViewGameProgress()
    {
        Selection.activeObject = Load(); // does not update if saved game progress is changed
    }

    [MenuItem("Game Save/Reset Game Progress", false, 100)]
    public static void ResetGameProgress()
    {
        // Confirm dialog
        if (!EditorUtility.DisplayDialog("Reset Game Progress", "Are you sure you want to reset game progress?", "Yes", "No")) return;

        Delete();
        Selection.activeObject = null;
    }

    [CustomEditor(typeof(GameProgress))]
    public class GameProgressEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var progress = (GameProgress) target;
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // Add save button
            var saveButton = new Button(() =>
            {
                GameProgress.Save(progress);
                Debug.Log("Game progress saved");
            });
            saveButton.text = "Save";
            root.Add(saveButton);

            return root;
        }
    }

    #endif
}
