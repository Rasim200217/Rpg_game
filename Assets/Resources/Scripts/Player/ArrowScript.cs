using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowSpeed = 2000f;
    private Rigidbody2D _rigidbody2D;
    private bool _isHited;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        if (!_isHited)
        {
            _rigidbody2D.velocity = transform.right * arrowSpeed * Time.deltaTime;
        }   
        else
        {
            _rigidbody2D.velocity *= 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Player" && !_isHited)
        {
            _isHited = true;
            GetComponent<Collider2D>().enabled = false;
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

            if(collision.transform.tag == "Enemy")
            {
                transform.SetParent(collision.transform);
                collision.transform.GetComponent<EnemyCanDie>().ArrowHit(transform.position);
            }
        }
    }
}
