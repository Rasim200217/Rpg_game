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


    [Header("Weapons")]
    /*sword*/
    public GameObject mySword;
    public Animator mySwordAnimator;
    public SpriteRenderer mySwordRender;
    public BoxCollider2D mySwordCollider;
    private bool _isMelee;

    [Space]
    /*box*/
    public GameObject distantWeapon;
    public SpriteRenderer myBow;
    public Transform arrowPoint; // точка появления стрелы при нажатии тетивы
    private float _bowReady;
    private bool _arrowIsReady;
    private bool _bowIsCharched;

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

        mySword = _mySprite.GetChild(0).gameObject;
        mySwordAnimator = mySword.GetComponent<Animator>();
        mySwordRender = mySword.GetComponent<SpriteRenderer>();
        mySwordCollider = mySword.GetComponent<BoxCollider2D>();

        distantWeapon = transform.GetChild(1).gameObject;
        myBow = distantWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrowPoint = distantWeapon.transform.GetChild(1);

        mySword.SetActive(false);
        distantWeapon.SetActive(false);
    }

     void Update()
    {
        InputManager();
        Dash();
        Attack();
    }

     void FixedUpdate()
    {
        if (!isHited && !isDashed) Move();
    }

    private void Attack()
    {
        if (Input.GetMouseButton(1)) Distant(); 
        else Melee();

        if (Input.GetMouseButtonUp(1)) {
            _bowIsCharched = false;
            _arrowIsReady = false;
            _bowReady = 0;
        }
     

        HeatOff();
    }

    private void Melee()
    {
        if (_isMelee) return;

        if (Input.GetMouseButtonDown(0) && Inventory.inventory.equipment[0])
        {
            if (PlayerStats.PlayerStaminaDamage(Inventory.inventory.equipment[0].weight, PlayerStats.Strenght,
                PlayerStats.stats.strWeight))
            {
                mySwordRender.sprite = Inventory.inventory.equipment[0].sprite;
                mySwordRender.size = new Vector2(0.4f, Inventory.inventory.equipment[0].lenght);
                mySwordRender.size = new Vector2(0f, Inventory.inventory.equipment[0].offset);

                float speed = Inventory.inventory.equipment[0].speed; // +буст от ловкости

                mySwordAnimator.speed = speed;

                _isMelee = true;
                mySword.SetActive(true);
            }
        }
    }

    private void Distant()
    {
        if (_isMelee || !Inventory.inventory.equipment[1]) return;

        distantWeapon.SetActive(true);


        Vector2 bowPos = distantWeapon.transform.position;
        Vector2 direction = (Vector2)_cursor - bowPos;
        distantWeapon.transform.right = direction;

        PlayerStats.staminaWait = 0;

        _bowReady += Inventory.inventory.equipment[1].speed * 0.5f * Time.deltaTime; // +буст от ловкости (0.2f)

        if (_bowReady <= 2)
        {
            if(myBow.sprite != Inventory.inventory.equipment[1].sprite)
            {
                myBow.sprite = Inventory.inventory.equipment[1].sprite;
            }

            if (_bowReady < 1f) arrowPoint.gameObject.SetActive(false);
            else
            {
                if (Inventory.inventory.ArrowChecked(Inventory.inventory.equipment[1].arrowId))
                {
                    if (!_arrowIsReady)
                    {
                        _arrowIsReady = true;
                        arrowPoint.gameObject.SetActive(true);
                        arrowPoint.localPosition = new Vector3(1.2f, 0, 0);
                        arrowPoint.GetComponent<SpriteRenderer>().sprite = Inventory.inventory.GetArrowSprite();
                    }
                }
            }
        }
        else if (_arrowIsReady)
        {
            if (!_bowIsCharched)
            {
                myBow.sprite = Inventory.inventory.equipment[1].spriteForBowState;
            }
            arrowPoint.localPosition = new Vector3(0.60f, 0, 0);
            _bowIsCharched = true;
        }

        if (_bowIsCharched)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (PlayerStats.PlayerStaminaDamage(Inventory.inventory.equipment[1].weight, PlayerStats.Strenght,
                PlayerStats.stats.strWeight))
                {
                _bowIsCharched = false;
                _arrowIsReady = false;
                _bowReady = 0f;
                Inventory.inventory.UseArrow();
                Instantiate(Inventory.inventory.equipment[1].myArrow, arrowPoint.position, arrowPoint.rotation);
                }
            }
        }
    }

    private void HeatOff()
    {
        if (!Input.GetMouseButton(1)) distantWeapon.SetActive(false);

        if (!_isMelee) return;

        if(!mySwordAnimator.GetCurrentAnimatorStateInfo(0).IsName("MeleeHeat"))
        {
            _isMelee = false;
            mySword.SetActive(false);
        }
    }

    void Move()
    {
        _mySprite.eulerAngles = new Vector3(0, 0, 15 * -x);
        _rigidbody2D.velocity = new Vector2(x, y) * Time.deltaTime * speed;

        if (_cursor.x < transform.position.x && _facingRight) Flip();
        else if (_cursor.x > transform.position.x && !_facingRight) Flip();
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_playerIsStand && !isDashed)
        {
            if (PlayerStats.PlayerStaminaDamage(dashStaminaLose, PlayerStats.Agility,
                PlayerStats.stats.aglDash))
            {
                isDashed = true;
                _dashTime = dashCountDown;
                _rigidbody2D.AddForce(new Vector2(xPlus * dashForse, yPlus * dashForse), ForceMode2D.Impulse);
            }
         }

        if (_dashTime > 0) _dashTime -= Time.deltaTime;
        else if(isDashed)
        {
            _dashTime = 0;
            isDashed = false;
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    void InputManager()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        xPlus = Input.GetAxis("Horizontal+");
        yPlus = Input.GetAxis("Vertical+");

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
