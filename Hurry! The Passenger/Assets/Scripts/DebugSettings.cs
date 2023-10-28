using UnityEngine;
using UnityEditor;

// Settings used for debugging, not included in version control
public class DebugSettings : ScriptableObject
{
    public bool showGuideWhenStart = true;
    public bool changeSpawningPoint = false;
    public Vector3 newSpawningPoint;
    public Vector3 newSpawningRotation;

    #if UNITY_EDITOR
    private static readonly string assetPath = "Assets/DebugSettings.asset";
    #endif

    private static DebugSettings instance_;
    public static DebugSettings instance
    {
        get
        {
            if (instance_ == null)
            {
                #if UNITY_EDITOR
                // Load or create an asset in editor
                instance_ = AssetDatabase.LoadAssetAtPath<DebugSettings>(assetPath);
                if (instance_ == null) CreateDebugSettingsAsset();
                #else
                // Use the default values in player
                instance_ = ScriptableObject.CreateInstance<DebugSettings>();
                #endif
            }
            return instance_;
        }
    }

    #if UNITY_EDITOR

    [MenuItem("Game Save/View Debug Settings", false, 2)]
    public static void ViewDebugSettings()
    {
        Selection.activeObject = instance;
    }

    [MenuItem("Game Save/Reset Debug Settings", false, 102)]
    public static void ResetDebugSettings()
    {
        // Confirm dialog
        if (!EditorUtility.DisplayDialog("Create Debug Settings", "Are you sure you want to recreate debug settings?", "Yes", "No")) return;

        CreateDebugSettingsAsset();
        Selection.activeObject = null;
    }

    private static void CreateDebugSettingsAsset()
    {
        instance_ = ScriptableObject.CreateInstance<DebugSettings>();
        AssetDatabase.CreateAsset(instance_, assetPath);
        AssetDatabase.SaveAssets();
    }

    #endif
}