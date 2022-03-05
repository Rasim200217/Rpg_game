using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Controller _controller;
    private SpriteRenderer _spriteRenderer;

  [SerializeField] private Color _hitColor;
    private Color _myColor;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _controller = GetComponent<Controller>();
        _spriteRenderer =transform.GetChild(0).GetComponent<SpriteRenderer>();

        _myColor = _spriteRenderer.color;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!_controller.isHited)
        {
            if (collision.transform.tag == "Enemy")
            {
                Hit(collision.transform.GetComponent<EnemyStats>());
            }
        }
    }

    private void Hit(EnemyStats enemyStats)
    {
        int damage = enemyStats.enemyDamage - PlayerStats.PlayerProtection / 2;
        if (damage <= 0) damage = 1;
        PlayerStats.stats.PlayerDamage(damage);

        StartCoroutine(Stun());

        _rigidbody2D.velocity = Vector2.zero;

        if (enemyStats.transform.position.x > transform.position.x)
        {
            _rigidbody2D.AddForce(Vector2.left * enemyStats.hitPuls, ForceMode2D.Impulse);
        }
        else _rigidbody2D.AddForce(Vector2.right * enemyStats.hitPuls, ForceMode2D.Impulse);
    }

    private IEnumerator Stun()
    {
        _controller.isHited = true;
        _spriteRenderer.color = _hitColor;

        yield return new WaitForSeconds(0.5f);

        _spriteRenderer.color = _myColor;
        _controller.isHited = false;
    }
}
