using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    public MainPanel mainPanel;
    public static bool mainPanelIsOpen;

    public RectTransform hpUI;
    public RectTransform mnUI;
    public RectTransform spUI;

    private Image hp;
    private Image mp;
    private Image sp;

    private Text hpText;
    private Text spText;
    private Text mpText;


    public static CanvasScript canvas;


    
    void Start()
    {
        canvas = this;

        if (hpUI)
        {
            hp = hpUI.GetChild(0).GetComponent<Image>();
        }
        if (mnUI)
        {
            mp = mnUI.GetChild(0).GetComponent<Image>();
        }
        if (spUI)
        {
            sp = spUI.GetChild(0).GetComponent<Image>();
        }


        AccessAll();
        mainPanelIsOpen = false;
    }

    private void FixedUpdate()
    {
        DataBars();
        SizeBars();

        if (Input.GetKey(KeyCode.Alpha1)) PlayerStats.stats.PlayerDamage(1);
        if (Input.GetKey(KeyCode.Alpha2)) PlayerStats.stats.PlayerManaDamage(1);
        if (Input.GetKey(KeyCode.Alpha3)) PlayerStats.stats.PlayerStaminaDamage(1);
    }

    private void DataBars()
    {
        hp.fillAmount = ((float)PlayerStats.PlayerHealth / PlayerStats.PlayerMaxHealth);
        mp.fillAmount = ((float)PlayerStats.PlayerMana / PlayerStats.PlayerMaxMana);
        sp.fillAmount = (PlayerStats.PlayerStamina / PlayerStats.PlayerMaxStamina);
    }

    private void SizeBars()
    {
        hpUI.sizeDelta = new Vector2(80 + PlayerStats.PlayerMaxHealth * 2, hpUI.sizeDelta.y);
        mnUI.sizeDelta = new Vector2(80 + PlayerStats.PlayerMaxMana * 2, mnUI.sizeDelta.y);
        spUI.sizeDelta = new Vector2(80 + PlayerStats.PlayerMaxStamina * 2, spUI.sizeDelta.y);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            mainPanelIsOpen = !mainPanelIsOpen;
            mainPanel.gameObject.SetActive(mainPanelIsOpen);
        }
    }


    private void AccessAll()
    {
        mainPanel.Access();
    }
}
