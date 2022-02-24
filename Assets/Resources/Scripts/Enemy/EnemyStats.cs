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
        Debug.Log("Enemy Died");
    }
}
