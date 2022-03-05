using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanDie : MonoBehaviour
{
    public Color hitColor;
    public float HitColorChangeTime = 0.3f;

    private Rigidbody2D _rigidbody2D;
    private EnemyStats _enemyStats;
    private EnemyAI _enemyAI;
    private SpriteRenderer _spriteRenderer;

    private Color _myColor;
    private bool _isHited;

     void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _enemyStats = GetComponent<EnemyStats>();
        _enemyAI = GetComponent<EnemyAI>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        _myColor = _spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Sword")) SwordHit();
    }

    private void SwordHit()
    {
        if (_isHited) return;

        // агрессия
        StartCoroutine(HitVisual());
        _enemyStats.SetDamage(PlayerStats.PlayerMelDamage);

        Transform player = Controller.con.transform;
        _rigidbody2D.velocity = Vector2.zero;

        if(player.position.x > transform.position.x)
        {
            _rigidbody2D.AddForce(Vector2.left * (Inventory.inventory.equipment[0].pulse));
        }
        else
        {
            _rigidbody2D.AddForce(Vector2.right * (Inventory.inventory.equipment[0].pulse));
        }

     }

    public void ArrowHit(Vector3 arrowPos)
    {
        if (_isHited) return;

        // агрессия
        StartCoroutine(HitVisual());
        _enemyStats.SetDamage(PlayerStats.PlayerDisDamage);

        _rigidbody2D.velocity = Vector2.zero;

        if (arrowPos.x > transform.position.x)
        {
            _rigidbody2D.AddForce(Vector2.left * (Inventory.inventory.equipment[1].pulse));
        }
        else
        {
            _rigidbody2D.AddForce(Vector2.right * (Inventory.inventory.equipment[1].pulse));
        }
    }

    private IEnumerator HitVisual()
    {
        _isHited = true;
        _enemyAI.canMove = false;
        _spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(HitColorChangeTime);

        _isHited = false;
        _enemyAI.canMove = true;
        _spriteRenderer.color = _myColor;
    }
}
