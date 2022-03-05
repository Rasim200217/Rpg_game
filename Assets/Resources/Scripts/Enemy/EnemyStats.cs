using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int enemyHealth;
    public int enemyDamage;
    public int hitPuls;

    [Header("Drop")]
    public int exp;
    public Loot[] loots;

    private GameObject _itemPref;

    private void Start()
    {
        _itemPref = Resources.Load<GameObject>("Prefabs/Other/Item");
    }

    public void SetDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0) Die();
    }

    private void Die()
    {
        for (int i = 0; i < loots.Length; i++)
        {
            if (!loots[i].item || loots[i].count <= 0)
            {
                Debug.LogWarning(loots[i] + ": Лоот под индексом " + i + ", объекта " + gameObject + ", указан неверно!");
                continue;
            }

            float random = Random.Range(0f, 100f);
            if (random <= loots[i].chance)
            {
                ItemSettings temp = Instantiate(_itemPref, Random.insideUnitSphere * 1.5f + transform.position, Quaternion.identity).GetComponent<ItemSettings>();
                temp.thisItem = loots[i].item;
                temp.count = loots[i].count;
            }   
        }

        PlayerStats.stats.AddExp(exp);
        Destroy(gameObject);
    }

    [System.Serializable]
    public struct Loot
    {
        public Item item;
        public int count;
        public float chance;
    }
}
