using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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
    private EquipCellScript[] equipCells;

    // Colors
    [Header("Inventory Colors")]
    public Color myColor;
    public Color cursorColor;
    public Color selectColor;
    public Color[] equipColor;

    GameObject _itemPref;

   public void Access()
   {
        _cellPanel = transform.GetChild(0);
        _infoPanel = transform.GetChild(1);
        _equipPanel = transform.GetChild(2);


        //cursor
        if (cursor)
        {
            _cursorImage = cursor.GetComponent<Image>();
            _cursorText = cursor.GetChild(0).GetComponent<Text>();
        }
        else Debug.Log("Нет ссылки на курсор");


        //inventory
        cells = new CellScript[25];
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = _cellPanel.GetChild(i).GetComponent<CellScript>().GetLinkSetting(i, this);
        }

        // info
        _infoImage = _infoPanel.GetChild(0).GetChild(0).GetComponent<Image>();
        _infoName = _infoPanel.GetChild(1).GetComponent<Text>();
        _infoDescription = _infoPanel.GetChild(2).GetComponent<Text>();
        _infoEffect = _infoPanel.GetChild(3).GetComponent<Text>();
        _infoCost = _infoPanel.GetChild(4).GetComponent<Text>();


        //equip
        equipCells = new EquipCellScript[3];
        for (int i = 0; i < equipCells.Length; i++)
        {
            equipCells[i] = _equipPanel.GetChild(i).GetComponent<EquipCellScript>();
        }

        _itemPref = Resources.Load<GameObject>("Prefabs/Other/Item");
   }

    void Update()
    {
        if(cursorCell)
        {
            if(Input.GetMouseButtonDown(0)) // нажалите лкм
            {
                SelectCellSwitch();
            }
            if (Input.GetMouseButtonDown(1)) // нажалите пкм
            {
                if (Inventory.inventory.Use(cursorCell.cellId)) RefreshAll();
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!cursorCell) return;
        if(!Inventory.inventory.items[cursorCell.cellId]) return;

        previosCell = cursorCell;
        selectedCell = cursorCell;
        RefreshAll();
        
        cursor.gameObject.SetActive(true);
        _cursorImage.sprite = Inventory.inventory.items[previosCell.cellId].sprite;
        _cursorText.text = Inventory.inventory.counts[previosCell.cellId].ToString();
    }
    public void OnDrag(PointerEventData eventData)
    {
        cursor.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cursorCell == previosCell)
        {

            ClearCursor();
            return;
        }
        if (!cursorCell && previosCell)
        {
            DropItem();
            ClearCursor();
        }
        if (cursorCell && previosCell)
        {
            if (cursorCell.isFree)
            {
                Inventory.inventory.MoveItem(previosCell.cellId, cursorCell.cellId);
            }
            else Inventory.inventory.SwapItem(previosCell.cellId, cursorCell.cellId);

            ClearCursor();
        }
    }

    public void DropItem()
    {
        int index = (int)Inventory.inventory.items[previosCell.cellId].myType;

        if(Inventory.inventory.equipment.Length > index)
        {
            if(Inventory.inventory.equipment[index] == Inventory.inventory.items[previosCell.cellId])
            {
                Inventory.inventory.equipment[index] = null;
                previosCell.isEquipt = false;
            }
        }

        Vector3 tempVec = Controller.con.transform.position + Random.insideUnitSphere * 1.5f;
        tempVec.z = -0.1f;

        ItemSettings temp = Instantiate(_itemPref, tempVec, Quaternion.identity).GetComponent<ItemSettings>();

        temp.thisItem = Inventory.inventory.items[previosCell.cellId];
        temp.count = Inventory.inventory.counts[previosCell.cellId];

        Inventory.inventory.items[previosCell.cellId] = null;
        Inventory.inventory.counts[previosCell.cellId] = 0;
    }

    private void ClearCursor()
    {
        cursor.gameObject.SetActive(false);
        previosCell = null;
        if(cursorCell) selectedCell = cursorCell;
        RefreshAll();
    }
    

    private void SelectCellSwitch()
    {
        if (!selectedCell) selectedCell = cursorCell;
        else
        {
            if(selectedCell == cursorCell)
            {
                selectedCell = null;
            }
            else
            {
                selectedCell = cursorCell;
            }
        }
        RefreshAll();
    }

    public void CursorCellSwitch(CellScript newCell)
    {
        if (!cursorCell) cursorCell = newCell;
        else cursorCell = null;

        RefreshAll();
    }

    public void RefreshAll()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].isEquipt = false;

            if(Inventory.inventory.items[i])
            {
                int index = (int)Inventory.inventory.items[i].myType;

                if(Inventory.inventory.equipment.Length > index) // Отрегулировать при добавлении новых типов предметов!
                {
                    if (Inventory.inventory.equipment[index] == Inventory.inventory.items[i]) cells[i].isEquipt = true;
                }
            }
            
            cells[i].Refresh();
            
            if (cells[i].isEquipt) cells[i].SetColor(equipColor[(int)Inventory.inventory.items[i].myType]);
            else cells[i].SetColor(myColor);
        }


        if (cursorCell && !cursorCell.isEquipt) cursorCell.SetColor(cursorColor);

        if (selectedCell)
        {
            if (!selectedCell.isEquipt) selectedCell.SetColor(selectColor);
            InfoChange(Inventory.inventory.items[selectedCell.cellId]);
        }
        else InfoChange();
        
        for (int i = 0; i < equipCells.Length; i++)
        {
            equipCells[i].Refresh();
        }
    }

    private void InfoChange(Item itemInfo = null)
    {
        if(itemInfo)
        {
            _infoImage.enabled = true;
            _infoImage.sprite = itemInfo.sprite;

            _infoName.text = itemInfo.itemName;
            _infoDescription.text = itemInfo.description;
            _infoEffect.text = itemInfo.effect;
            _infoCost.text = "Цена: " + itemInfo.cost;
        }
        else
        {
            _infoImage.enabled = false;

            _infoName.text = "";
            _infoDescription.text = "";
            _infoEffect.text = "";
            _infoCost.text = "";
        }
    }


    public void Cleaner()
    {
        ClearCursor();
        cursorCell = null;
        selectedCell = null;
        RefreshAll();
    }

    
}
