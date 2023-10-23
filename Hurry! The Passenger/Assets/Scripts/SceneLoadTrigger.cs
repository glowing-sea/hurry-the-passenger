using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SceneLoadTrigger : MonoBehaviour
{
    public enum SceneLoadTriggerType
    {
        Load,
        Unload
    }
    public SceneLoadTriggerType type;

    public bool destroyAfterUse = false;

    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        switch (type)
        {
            case SceneLoadTriggerType.Load:
                GameManager.instance.LoadScene(sceneName);
                break;
            case SceneLoadTriggerType.Unload:
                GameManager.instance.UnloadScene(sceneName);
                break;
        }

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
