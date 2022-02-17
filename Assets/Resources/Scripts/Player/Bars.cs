using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
 public Image Healthbar;
 public Image Manabar;
 public Image Agilitybar;
 [Range(0, 1f)]
 public float fillHealth;
 [Range(0, 1f)]
 public float fillMana;
 [Range(0, 1f)]
 public float fillAgility;

 private void Start()
 {
  fillHealth = 1f;
  fillMana = 1f;
  fillAgility = 1f;
 }

 private void Update()
 {
  Healthbar.fillAmount = fillHealth;
  Manabar.fillAmount = fillMana;
  Agilitybar.fillAmount = fillAgility;
 }
}
