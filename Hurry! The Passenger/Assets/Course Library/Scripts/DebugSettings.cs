using UnityEngine;
using UnityEditor;

// Settings used for debugging, not included in version control
public class DebugSettings : ScriptableObject
{
    public bool showGuideWhenStart = true;
    public bool changeSpawningPoint = false;
    public Vector3 newSpawningPoint;
    public Vector3 newSpawningRotation;

    private static readonly string assetPath = "Assets/DebugSettings.asset";

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
    [MenuItem("Game Settings/(Re)craete Debug Settings", false, 20)]
    public static void CreateDebugSettingsAsset()
    {
        instance_ = ScriptableObject.CreateInstance<DebugSettings>();
        AssetDatabase.CreateAsset(instance_, assetPath);
        AssetDatabase.SaveAssets();
    }
    #endif
}