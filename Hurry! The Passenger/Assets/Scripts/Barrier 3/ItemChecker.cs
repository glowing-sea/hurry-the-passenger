using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
            if (attributes.itemType == ItemType.Normal && attributes.onTheTray)
            {
                SecurityCheckMinigame.instance.DecrementItemNum();
                GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.taskComplete, 1f);
            } else
            {
                GameManager.instance.sfxPlayer.PlayOneShot(GameManager.instance.somethingWrong, 1f);
            }
        }
    }
}
