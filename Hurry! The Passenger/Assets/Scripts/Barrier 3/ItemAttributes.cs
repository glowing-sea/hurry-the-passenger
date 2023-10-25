using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttributes : MonoBehaviour
{
    public ItemType itemType;
    public bool onTheTray = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

}

public enum ItemType
{
    Normal,
    Danger,
    Liquid
}
