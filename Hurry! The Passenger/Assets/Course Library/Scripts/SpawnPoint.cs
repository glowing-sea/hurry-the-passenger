using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each scene should have one and only one spawn point,
/// which is a game object tagged with Respawn and has a SpawnPoint script attached.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Find the spawn point in the given scene.
    /// </summary>
    public static SpawnPoint FindInScene(string sceneName)
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint.scene.name == sceneName)
            {
                return spawnPoint.GetComponent<SpawnPoint>();
            }
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(0, 1f, 0), new Vector3(0.8f, 2f, 0.8f));
        Gizmos.DrawWireSphere(new Vector3(0, 2.5f, 0), 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(new Vector3(0, 2.5f, 0), Vector3.forward * 2f);
    }
}
