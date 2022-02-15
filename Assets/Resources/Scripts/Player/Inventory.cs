using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    public Item[] equipment;

    public int[] counts;
    public int money;

    private int _arrowId;

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


    public bool ArrowChecked(int id)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i])
            {
                if (items[i].id == id)
                {
                    _arrowId = i;
                    return true;
                }
            }
        }
        _arrowId = 0;
        return false;
    }

    public Sprite GetArrowSprite()
    {
        return items[_arrowId].sprite;
    }

    public void UseArrow()
    {
        counts[_arrowId]--;

        if (counts[_arrowId] <= 0) items[_arrowId] = null;
    }

    public void MoveItem(int oldId, int newId)
    {
        items[newId] = items[oldId];
        counts[newId] = counts[oldId];

        items[oldId] = null;
        counts[oldId] = 0;
    }

    public void SwapItem(int oldId, int newId)
    {
        Item tempItem = items[newId];
        int tempCount = counts[newId];

        items[newId] = items[oldId];
        counts[newId] = counts[oldId];

        items[newId] = tempItem;
        counts[newId] = tempCount;
    }
}
