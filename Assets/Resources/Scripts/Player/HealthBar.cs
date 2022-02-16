using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
 public Image Healthbar;
 public float fill;

 private void Start()
 {
  fill = 1f;
 }

 private void Update()
 {
  Healthbar.fillAmount = fill;
 }
}
