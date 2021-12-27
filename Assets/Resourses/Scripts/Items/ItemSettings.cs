﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSettings : MonoBehaviour
{
    public Item thisItem;
    public int count = 1;
   
    void Start()
    {
        gameObject.name = thisItem.itemName;
        GetComponent<SpriteRenderer>().sprite = thisItem.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.GetComponent<Interactive>().item = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.GetComponent<Interactive>().item = null;
        }
    }
}
