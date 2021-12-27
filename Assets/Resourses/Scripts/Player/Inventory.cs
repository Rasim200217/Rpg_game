using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    public Item[] equipment;

    public int[] counts;
    public int[] money;

    public static Inventory inventory;

    private void Awake() //место в инвентаре
    {
        inventory = this;

        items = new Item[25];
        counts = new int[25];

        equipment = new Item[3];
    }

   public bool AddItem(Item newItem, int newCount)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] && items[i].id == newItem.id)
            {
                counts[i] += newCount;
                return true;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i])
            {
                items[i] = newItem;
                counts[i] = newCount;
                return true;
            }
        }
        return false;
    }
}
