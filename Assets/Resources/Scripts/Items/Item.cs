using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName = "Предмет";
    public Sprite sprite;
    [TextArea]
    public string description = "Здесь будет атмосферное описание";
    [TextArea]
    public string effect = "Здесь должно быть описание свойства предмета";
    public int cost;

    public enum ItemsTypes
    {
        melWeapon = 0,
        disWeapon = 1,
        armor = 2,
        item = 3,
        gold = 4
    }
    public ItemsTypes myType = ItemsTypes.item;

    [Header("Item Settings")]
    public bool isUseful;
    public bool isArrow;
    public int arrowId;

    [Header("Weapon Settings")]
    public int damage;
    public float pulse;
    public float speed;
    public float weight;
    [Space] // Melee
    public float lenght;
    public float offset;
    [Space] // Distant
    public Sprite spriteForBowState;
    public GameObject myArrow;

    [Header("Armor Settings")]
    public int protection;

    [Header("Book Settings")]
    public Spell bookSpell;
}
