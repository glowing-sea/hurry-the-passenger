using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttributes : MonoBehaviour
{
    public ItemType itemType;
    public bool onTheTray = false;

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    [SerializeField] private Material blue;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        switch (itemType)
        {
            case ItemType.Normal:
                rend.sharedMaterial = green;
                break;
            case ItemType.Danger:
                rend.sharedMaterial = red;
                break;
            case ItemType.Liquid:
                rend.sharedMaterial = blue;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EmptyLiquid()
    {
        if(itemType == ItemType.Liquid)
        {
            itemType = ItemType.Normal;
            rend.sharedMaterial = green;
        }
    }

    public void MarkAsRescan()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}

public enum ItemType
{
    Normal,
    Danger,
    Liquid
}
