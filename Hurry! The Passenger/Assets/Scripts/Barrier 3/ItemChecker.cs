using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    SecurityCheckMinigame minigame;

    // Start is called before the first frame update
    void Start()
    {
        minigame = SecurityCheckMinigame.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ItemAttributes attributes = other.gameObject.GetComponent<ItemAttributes>();

            switch (attributes.itemType)
            {
                case ItemType.Normal:
                    if (attributes.onTheTray)
                    {
                        SecurityCheckMinigame.instance.DecrementItemLeft();
                        GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.taskComplete, 1f);
                    }
                    else
                    {
                        minigame.ShowWarning("I should put it on the tray first");
                        minigame.itemsToBeReScanned.Enqueue(attributes.itemType);
                        minigame.TimeDecreasePunish();
                        Destroy(other.gameObject);
                    }
                    break;
                case ItemType.Liquid:
                    minigame.ShowWarning("I should empty it befor scanning");
                    minigame.itemsToBeReScanned.Enqueue(attributes.itemType);
                    Destroy(other.gameObject);
                    minigame.TimeDecreasePunish();
                    break;
                case ItemType.Danger:
                    minigame.ShowWarning("This is a prohibited item, I shouldn't try taking it on the plane");
                    minigame.itemsToBeReScanned.Enqueue(attributes.itemType);
                    Destroy(other.gameObject);
                    minigame.TimeDecreasePunish();
                    break;
            }
        }
    }
}
