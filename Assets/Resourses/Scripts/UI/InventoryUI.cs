using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryUI : MonoBehaviour
{
    public CellScript[] cells; // ссылки на ячейки инвенторя
    private Transform _cellPanel; // ссылка на главную панель ячеек (для сокращение ссылок)
    public Transform cursor; // ссылка на объект привязанный к курсору
    private Image _cursorImage;
    private Text _cursorText;

    // Interactive Settings
    public Item selectedItem; // сохраненный, выделенный предмет
    public int selectedCount; // сохраненное кол-во выделенного предмета
    public CellScript cursorCell; // выделенная ячейка курсором
    public CellScript selectedCell; // выбранная ячейка
    public CellScript previosCell; // стартовая ячейка перетаскивания

    // InfoPanel Settings
    private Transform _infoPanel; // ссылка на панель информации (для сокращение ссылок)
    private Image _infoImage;
    private Text _infoName;
    private Text _infoDescription;
    private Text _infoEffect;
    private Text _infoCost;

    // EquipPanel Settings
    private Transform _equipPanel;

    // Colors
    [Header("Inventory Colors")]
    public Color myColor;
    public Color cursorColor;
    public Color selectColor;
    public Color equipColor;

    private GameObject _itemPref;

   public void Access()
   {
        _cellPanel = transform.GetChild(0);

        cells = new CellScript[25];
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = _cellPanel.GetChild(i).GetComponent<CellScript>().GetLinkSetting(i, this);
        }
   }

    void Update()
    {

    }

    public void CursorCellSwitch(CellScript newCell)
    {
        if (!cursorCell) cursorCell = newCell;
        else cursorCell = null;
    }

    public void RefreshAll()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Refresh();
        }
    }
}
