using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference <https://www.youtube.com/watch?v=28JTTXqMvOU>

public class Minimap : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        // transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
