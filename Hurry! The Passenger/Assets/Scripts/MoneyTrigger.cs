using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTrigger : MonoBehaviour
{
    public float spinSpeed;
    // how much money the player will get when they hit this object
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            GameManager.instance.UpdateBalance(value, true);
            Destroy(gameObject);
        }
    }
}
