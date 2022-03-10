using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSettings : MonoBehaviour
{
    public Dialog dialog;
    public Dialog dialogEnd;

    public bool isShop;

    [Header("Debug")]
    public bool dialogueEnded;
    public bool dialogueStarted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            if (dialogueEnded && dialogEnd == null) return;
            Debug.Log("can start dialogue" + dialog.npcName);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            Debug.Log("can start dialogue" + dialog.npcName);
        }
    }
}
