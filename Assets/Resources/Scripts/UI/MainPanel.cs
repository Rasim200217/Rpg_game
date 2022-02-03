using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public GameObject[] Panels;

    private StatsUI _stats;
    private InventoryUI _inventoryUI;
    private SpellBookUI _spellBookUI;

    public void Access()
    {
        Panels = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            Panels[i] = transform.GetChild(i).gameObject;
        }

        _stats = Panels[0].GetComponent<StatsUI>();
        _inventoryUI = Panels[1].GetComponent<InventoryUI>();
        _spellBookUI = Panels[2].GetComponent<SpellBookUI>();

        _inventoryUI.Access();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_inventoryUI) _inventoryUI.Cleaner();
    }

    private void OnDisable()
    {
        if (_inventoryUI) _inventoryUI.Cleaner();
    }

    public void Button(int index)
    {
        for (int i = 0; i < Panels.Length; i++)
        {
            Panels[i].SetActive(i == index);
        }
    }

}
