using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{

    public MainPanel mainPanel;
    public static bool mainPanelIsOpen;

    public static CanvasScript canvas;


    
    void Start()
    {
        canvas = this;
        mainPanelIsOpen = false;
        AccessAll();
    }

   
    void Update()
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
