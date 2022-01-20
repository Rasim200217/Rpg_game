using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    public Item[] equipment;

    public int[] counts;
    public int money;

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

    public void AddGold(int count)
    {
        Debug.Log("Вы подняли золото в размере" + count + ".");
        money += count;
    }

    public bool Use(int id)
    {
        if (!items[id]) return false;

        switch (items[id].myType)
        {
            case Item.ItemsTypes.item:
                return UseItem(id);
            default: SetEquip(items[id].myType, id);
                return true;
        }
    }

    private bool UseItem(int id)
    {
        if (!items[id].isUseful) return false;

        if(counts[id] > 1)
        {
            counts[id]--;
        }
        else
        {
            counts[id] = 0;
            items[id] = null;
        }
        return true;
    }

    private void SetEquip(Item.ItemsTypes equipType, int id)
    {
        if (equipment[(int)equipType] == items[id]) equipment[(int)equipType] = null;
        else equipment[(int)equipType] = items[id];
    }
}
