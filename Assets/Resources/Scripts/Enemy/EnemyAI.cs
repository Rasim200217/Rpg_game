using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType 
    { 
        Simple = 0,
        Shooter = 1,
        Big = 2
    }
    public EnemyType myType = EnemyType.Simple;

    [Header("Move Settings")]
    public float speed; // скорость преследования
    public float forceSpeed; // скорость рывка
    public float forceCountdown; // время перезарядки рывка
    public float patrolSpeed; // скорость брождения

    [Header("Radius Settengs")]
    public float chasingRadius; // радиус старта преследования
    public float attackRadius; // радиус дальний атаки (Дальний)
    public float retreatRadius; // радиус старта отступления (Дальний)
    public float maxX, minX, maxY, minY;

    [Header("Shooter Settings")]
    public float fireRite;
    public GameObject bulletPref;

    [Header("Unity Parameters")]
    public bool moveRight = false;
    public bool moveStop = false;
    public bool canMove = true;

    //Other

    private Transform _target; // цель преследования
    private Rigidbody2D _rigidbody; // ссылка на rigidbody
    private SpriteRenderer _mySprite; // ссылка на компонент спрайта чаелда объекта
    private EnemyStats _myStats; // ссылка на EnemyStats

    private bool _facingRight; // куда смотрит враг
    private bool _isForced; // противник совершил рывок
    private float _mySpeed; // настоящая скорость противника

    private Vector3 _startPos; // стартовая позиция противника
    private Vector3 _movePos; // точка движения
    
    private bool _gameStarted; // игра начиналось (Дебаг)

    private void Start()
    {
        _gameStarted = true;
        _startPos = transform.position;

        _rigidbody = GetComponent<Rigidbody2D>();
        _myStats = GetComponent<EnemyStats>();
        _mySprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _mySpeed = speed;

        MovePosCreator();

    }



    private void FixedUpdate()
    {
        if (canMove) AIChecker();
    }

    private void AIChecker()
    {
        if (myType == EnemyType.Simple) Searching();
        else if (myType == EnemyType.Big) Searching();
        else if (myType == EnemyType.Shooter) return;
    }

    private void Searching()
    {
        if (Vector2.Distance(transform.position, _target.position) <= chasingRadius) Chasing();
        else Patrol();
    }

    private void Chasing()
    {
        Flip(_target.position);

        Vector2 temp = Vector2.MoveTowards(transform.position, _target.position, _mySpeed * Time.deltaTime);
        _rigidbody.MovePosition(temp);

        if (!_isForced) StartCoroutine(Force());
    }

    private IEnumerator Force()
    {
        _isForced = true;
        _mySpeed = 0.5f;
        yield return new WaitForSeconds(0.5f);
        _mySpeed = forceSpeed;
        yield return new WaitForSeconds(0.2f);
        _rigidbody.AddForce(transform.forward * _mySpeed, ForceMode2D.Impulse);
        _mySpeed = speed;
        yield return new WaitForSeconds(forceCountdown);
        _isForced = false;
    }

    private void Patrol()
    {
        Flip(_movePos);

        Vector2 temp = Vector2.MoveTowards(transform.position, _movePos, patrolSpeed * Time.deltaTime);
        _rigidbody.MovePosition(temp);

        if (Vector2.Distance(transform.position, _movePos) <= patrolSpeed * Time.deltaTime)
        {
            MovePosCreator();
        }
    }

    private void MovePosCreator()
    {
        _movePos = new Vector2(_startPos.x + Random.Range(-minX, maxX), _startPos.y + Random.Range(-minY, maxY));

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_movePos, 0.3f);

        foreach (Collider2D temp in colliders)
        {
            if(temp.tag.Contains("Wall"))
            {
                _movePos = transform.position;
            }
        }
    }

    private void Flip(Vector3 flipTarget)
    {
        if (transform.position.x > flipTarget.x && _facingRight) Flipper();
        if (transform.position.x < flipTarget.x && !_facingRight) Flipper();

        void Flipper()
        {
            _facingRight = !_facingRight;
            Vector3 temp = _mySprite.transform.localScale;
            temp.x *= -1;
            _mySprite.transform.localScale = temp;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (!_gameStarted) _startPos = transform.position;

        Gizmos.color = Color.green;

        float yMod = (maxY - minY) / 2;
        float xMod = (maxX - minX) / 2;

        Vector3 pos = new Vector2(_startPos.x + xMod, _startPos.y + yMod);
        Vector3 size = new Vector3(maxX + minX, maxY + minY, 0.1f);

        Gizmos.DrawWireCube(pos, size);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasingRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_movePos, 0.15f);
    }



}
