using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The part of code of dragging objects with mouse modified from <https://www.youtube.com/watch?v=yalbvB84kCg>

// This script
public class Draggable : MonoBehaviour
{
    // Variables used for drag the object
    Vector3 mousePositionOffset;
    public Camera cam;

    // Collider of the item, used for calculated boundary
    private Collider boxCollider;

    // Change items when it is placed in different bags
    public Material grey;
    public Material yellow;
    public Material green;
    public Material blue;
    private Renderer rend;

    // The weight of the item
    public int weight;
    public TextMeshPro weightText;


    // Bounds of the bag, used to check if the draggable object is in one of the bag
    const float topBoundBag = 19.355f;
    const float bottomBoundBag = 16.52f;

    const float leftBoundBag1 = -6.80f;
    const float rightBoundBag1 = -4.52f;

    const float leftBoundBag2 = -3.53f;
    const float rightBoundBag2 = -1.27f;

    const float leftBoundBag3 = -0.46f;
    const float rightBoundBag3 = 1.81f;

    // Which bag the item is in.
    // 0: not in the bag, 1: in bag 1, 2: in bag 2, 3: in bag 3
    public int inWhichBag = 0;


    // Check if the item is overlapping with another item (to avoid overlapping)
    public bool isOverLap;

    // Start is called before the first frame update
    void Start()
    {
        // Initilise rending (to change colour)
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        // Get box collider reference (to get bound)
        boxCollider = GetComponent<Collider>();

        weightText.text = weight + " kg";
    }

    // Get the mouse world position
    private Vector3 GetMousePosition()
    {
        // Turn mouse position from relative to the screen to relative to the world
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // Keep checking which bag the item is in and change the item' colour accordingly.
    void Update()
    {
        InWhichBag();
        ChangeColour();
    }

    // This function is used to drag an object
    private void OnMouseDown()
    {
        // Capture the mouse offset
        mousePositionOffset = gameObject.transform.position - GetMousePosition();
    }

    // This function is used to drag an object
    private void OnMouseDrag()
    {
        transform.position = GetMousePosition() + mousePositionOffset;
        // InWhichBag();
    }

    // Check if the object is in the bag according the bound of the object and the bounds of the bags
    private void InWhichBag()
    {
        // Bounds of the draggable object
        float upBound = boxCollider.bounds.center.y + boxCollider.bounds.extents.y;
        float lowBound = boxCollider.bounds.center.y - boxCollider.bounds.extents.y;
        float rightBound = boxCollider.bounds.center.x + boxCollider.bounds.extents.x;
        float leftBound = boxCollider.bounds.center.x - boxCollider.bounds.extents.x;
        // Debug.Log(upBound + "  " + lowBound + "  " + leftBound + "  " + rightBound);

        if (upBound <= topBoundBag && lowBound >= bottomBoundBag && !isOverLap) // may be in one of the bag
        {
            // In bag 1
            if (rightBound <= rightBoundBag1 && leftBound >= leftBoundBag1)
            {
                inWhichBag = 1;
            }
            // In bag 2
            else if (rightBound <= rightBoundBag2 && leftBound >= leftBoundBag2)
            {
                inWhichBag = 2;
            }
            // In bag 3
            else if (rightBound <= rightBoundBag3 && leftBound >= leftBoundBag3)
            {
                inWhichBag = 3;
            }
            else // Not in any bag
            {
                inWhichBag = 0;
            }
        }
        else // Not in any bag
        {
            inWhichBag = 0;
        }
    }

    // Call when two items overlap
    private void OnTriggerStay(Collider other)
    {
        isOverLap = true;
    }

    // Call when the item does not overlap with others
    private void OnTriggerExit(Collider other)
    {
        isOverLap = false;
    }

    // Change the colour of the item according to which bag it is in.
    private void ChangeColour()
    {
        switch (inWhichBag)
        {
            case 1:
                rend.sharedMaterial = yellow;
                break;
            case 2:
                rend.sharedMaterial = green;
                break;
            case 3:
                rend.sharedMaterial = blue;
                break;
            default:
                rend.sharedMaterial = grey;
                break;
        }
    }
}