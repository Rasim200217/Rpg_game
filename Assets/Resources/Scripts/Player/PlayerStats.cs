using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats stats;

    public static string PlayerName;
    public static Sprite PlayerSprite;

    public static int PlayerHealth;
    public static int PlayerMaxHealth;
    public static int PlayerMana;
    public static int PlayerMaxMana;
    public static float PlayerStamina;
    public static float PlayerMaxStamina;

    public static int Strenght;
    public static int Agility;
    public static int Constitution;
    public static int Intelligence;

    public static int level = 1;
    public static int Exp;
    public static int ExpPoints;
    public static int PlayerProtection;
    public static int PlayerMelDamage;
    public static int PlayerDisDamage;
    public static int PlayerMagDamage;

    public int[] levelChart;

    [Header("Start Parameters")]
    public int StartStrenght = 1;
    public int StartAgiglity = 1;
    public int StartConstitution = 1;
    public int StartIntelligence = 1;
    public int StartExpPoints = 0;

    [Header("Bonus Parameters")]
    public int strDam = 3; // Урон за каждое очко силы
    public float strWeight = 0.25f; // Снижение веса оружия за каждое очко силы
    public int strStam = 4; // Бонус к стамине за каждое очко силы
    [Space]
    public int aglDam = 3; // Урон ха каждое очко ловкости
    public float aglSpeed = 0.1f; // Повышение скорости атаки закаждое очко ловкости
    public float aglDash = 0.25f; // Снижение траты выносливасти во время рывка за каждое очко ловкости
    public int aglStam = 4; // Бонус к стамине за каждое очко ловкости
    [Space]
    public int conHealth = 10; // Бонус к здоровью за каждое очко стойкости
    public int conProtect = 1; // Бонус к защите за каждое очко стойкости
    public int requireHealthRegen = 5; // Значение стойкости необходимое для активации регена здоровья
    public float timeHealthRegen = 0.1f; // Бонус к скорости регенерации за каждое очко стойкости
    [Space]
    public int intMana = 5; // Бонус к мане за каждое очко интелекта
    public int intMag = 2; // Бонус к маг урону за каждое очко интелекта
    public int requireManaRegen = 5; // Значение стойкости необходимое для активации регена маны
    public float timeManaRegen = 0.1f; // Бонус к скорости регенерации за каждое очко интелекта
    [Space]
    public int lvlHealth = 10; // Бонус к здоровью за каждое очко уровня
    public int lvlMana = 5; // Бонус к мане за каждое очко уровня
    public int lvlStamina = 2; // Бонус к выносливости за каждое очко уровня


    private void Awake()
    {
        stats = this;
    }

    private void Start()
    {
        Strenght = StartStrenght;
        Agility = StartAgiglity;
        Constitution = StartConstitution;
        Intelligence = StartIntelligence;

        Manager();

        Exp = 0;
        ExpPoints = StartExpPoints;

        PlayerHealth = PlayerMaxHealth;
        PlayerMana = PlayerMaxMana;
        PlayerStamina = PlayerMaxStamina;
    }

    private void FixedUpdate()
    {
        Manager();
    }

    private void Manager()
    {
        SetMaxParameters();
        SetDamageParameters();
        SetProtectionParameters();
    }

    private void SetMaxParameters()
    {
        if (PlayerHealth > PlayerMaxHealth) PlayerHealth = PlayerMaxHealth;
        if (PlayerMana > PlayerMaxMana) PlayerMana = PlayerMaxMana;
        if (PlayerStamina > PlayerMaxStamina) PlayerStamina = PlayerMaxStamina;

        if (PlayerStamina < 0) PlayerStamina = 0;
        if (PlayerMana < 0) PlayerMana = 0;

        // Regen for Stamina

        PlayerMaxHealth = 10 + (lvlHealth * level) + (conHealth * Constitution);
        PlayerMaxMana = 10 + (lvlMana * level) + (intMana * Intelligence);
        PlayerMaxStamina = 20 + (lvlStamina * level) + (strStam * Strenght) + (aglStam * Agility);

    }

    private void SetDamageParameters()
    {
        PlayerMelDamage = (strDam * Strenght);
        PlayerDisDamage = (aglDam * Agility);
        PlayerMagDamage = (intMag * Intelligence);

        if (Inventory.inventory.equipment[0]) PlayerMelDamage += Inventory.inventory.equipment[0].damage;
        if (Inventory.inventory.equipment[1]) PlayerDisDamage += Inventory.inventory.equipment[1].damage;
    }

    private void SetProtectionParameters()
    {
        PlayerProtection = level + (conProtect * Constitution);
        if (Inventory.inventory.equipment[2]) PlayerProtection += Inventory.inventory.equipment[2].protection;
    }

    private void CheckLevel()
    {
        if(Exp >= levelChart[level])
        {
            level++;
            ExpPoints += 2;
            SetMaxParameters();

            PlayerHealth = PlayerMaxHealth;
            PlayerMana = PlayerMaxMana;
            PlayerStamina = PlayerMaxStamina;
        }
    }

    public void PlayerDamage(int damage)
    {
        PlayerHealth -= damage;

        if(PlayerHealth <= 0)
        {
            PlayerHealth = 0;
        }
    }

    public void PlayerManaDamage(int damage)
    {
        PlayerMana -= damage;

        if (PlayerMana <= 0)
        {
            PlayerMana = 0;
        }
    }

    public void PlayerStaminaDamage(int damage)
    {
        PlayerStamina -= damage;

        if (PlayerStamina <= 0)
        {
            PlayerStamina = 0;
        }
    }

    private void HealthRegen()
    {

    }
    private void ManaRegen()
    {

    }
    private void StaminaRegen()
    {

    }
}
