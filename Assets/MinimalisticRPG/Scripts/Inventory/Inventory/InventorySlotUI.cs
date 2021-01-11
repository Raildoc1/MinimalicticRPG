using KG.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KG.UI
{
    [RequireComponent(typeof(Image))]
    public class InventorySlotUI : MonoBehaviour
    {

        public int index = 0;
        public Image image;
        public GameObject equipedIcon;

        public InventoryGridUI inventoryGridUI;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void OnClick()
        {
            inventoryGridUI.OnClick(index);
        }
    }
}

