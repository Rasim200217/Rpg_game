using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private InventoryUI ui;
    public int cellId = 0;

    public bool isFree = true;
    public bool isEquipt = false;

    private Image _myImage;
    private Image _myIcon;
    private Text _myCount;

    public void Refresh()
    {
        if (!Inventory.inventory) return;

        if(Inventory.inventory.items[cellId])
        {
            isFree = false;
            _myIcon.gameObject.SetActive(true);
            _myCount.gameObject.SetActive(true);

           _myIcon.sprite = Inventory.inventory.items[cellId].sprite;
           _myCount.text = Inventory.inventory.counts[cellId].ToString();
        }
        else
        {
            isFree = true;
            _myIcon.gameObject.SetActive(false);
            _myCount.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui) ui.CursorCellSwitch(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui) ui.CursorCellSwitch(this);
    }

    public void SetColor(Color newColor)
    {
        _myImage.color = newColor;
    }

    public CellScript GetLinkSetting(int newId, InventoryUI newUI)
    {
        ui = newUI;
        cellId = newId;

        isFree = true;
        _myImage = GetComponent<Image>();
        _myIcon = transform.GetChild(0).GetComponent<Image>();
        _myCount = transform.GetChild(1).GetComponent<Text>();

        return this;
    }
}
