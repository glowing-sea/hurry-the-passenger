using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to share between scene to reflect on game setting set on the titile screen
public class GameSettings : MonoBehaviour
{
    // Default sensitivity
    public static int sensitivity = 200;
    public static bool showGuideWhenStart = false;
    public static bool changeSpawningPoint = true;
    public static Vector3 newSpawningPoint = new Vector3(38, 0, 0);
    public static Quaternion newSpawningAngle = Quaternion.Euler(0, -90, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
