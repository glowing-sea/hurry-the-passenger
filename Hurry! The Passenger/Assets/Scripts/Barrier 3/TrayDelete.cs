using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayDelete : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tray"))
        {
            Destroy(collision.gameObject);
        }
    }
}
