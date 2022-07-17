using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
   [SerializeField] private Image itemImage;
   [SerializeField] private TMP_Text quantityTxt;

   [SerializeField] private Image borderImage;

   public bool mouseOver = false;
   private bool empty = true;
   private Camera mainCamera;

   public void Awake()
   {

      ResetData();
      Deselect();
   }
   public void ResetData()
   {
      this.itemImage.gameObject.SetActive(false);
      empty = true;
   }

   public void Deselect()
   {
      borderImage.enabled = false;
   }

   public void SetData(Sprite sprite, int quantity)
   {
      this.itemImage.gameObject.SetActive(true);
      this.itemImage.sprite = sprite;
      this.quantityTxt.text = quantity + "";
      empty = false;
   }

   public void Select()
   {
      borderImage.enabled = true;
   }

   private void OnTriggerEnter(Collider mouse)
   {
      if (mouse.gameObject.tag == "Mouse")
      {
         mouseOver = true;
      }
   }

   private void OnTriggerExit(Collider mouse)
   {
      if (mouse.gameObject.tag == "Mouse")
      {
         mouseOver = false;
      }
   }
}