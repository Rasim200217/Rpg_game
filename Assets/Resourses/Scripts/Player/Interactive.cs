using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemSettings item;
    // public Chest item;
    // public DialogSettings dialog;
    // public Save save;
    // public Door door;

    public static Interactive player;
    private Inventory inventory;

    private void Awake()
    {
        player = this;
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if(item)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                // Sound
                if (item.thisItem.myType == Item.ItemsTypes.gold) TakeGold();
                else TakeItem();
            }
        }
    }

    public void TakeItem()
    {
        if (inventory.AddItem(item.thisItem, item.count))
        {
            Debug.Log("Вы подняли " + item.thisItem.itemName);
            Destroy(item.gameObject);
            item = null;
        }
        else
        {
            Debug.Log("В инвенторе нет места!");
        }
    }

    public void TakeGold()
    {
        inventory.AddGold(item.count * item.thisItem.cost);
        Destroy(item.gameObject);
    }
}
