using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed; // скорость персонажа
    public float dashForse; // сила толчка
    public float dashStaminaLose; // трата выносливости при уклонении
    public float dashCountDown; // перерыв между уклонениями
    private float _dashTime;
    private float _dashReade;
    public bool isDashed; // определяет состояния уклонения

    private bool _facingRight = true; // куда смотрит персонаж
    public bool isHited; // определяет состояния получения урона
    private bool _playerIsStand; // определяет состояние НЕ движения


    [Header("Weapon")]
    public GameObject mySword;
    public GameObject myBow;
    public GameObject bowPoint;

    public Transform arrowPoint; // точка появления стрелы при нажатии тетивы

    //System
    private Transform _mySprite;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _cursor; // отслеживания позиции курсора
    private float x, y, xPlus, yPlus; // отслеживания Axis: Hor, Ver, Hor+, Ver+ 

    public static Controller con;

    void Awake()
    {
        con = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _mySprite = transform.GetChild(0);
    }

     void Update()
    {
        InputManager();
    }

     void FixedUpdate()
    {
        if (!isHited && !isDashed) Move();
    }

    void Move()
    {
        _mySprite.eulerAngles = new Vector3(0, 0, 15 * -x);
        _rigidbody2D.velocity = new Vector2(x, y) * Time.deltaTime * speed;

        if (_cursor.x < transform.position.x && _facingRight) Flip();
        else if (_cursor.x > transform.position.x && !_facingRight) Flip();
    }


    void InputManager()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (x == 0 && y == 0) _playerIsStand = true;
        else _playerIsStand = false;

        _cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    } 

    void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 temp = _mySprite.localScale;
        temp.x *= -1;
        _mySprite.localScale = temp;
    }
}
