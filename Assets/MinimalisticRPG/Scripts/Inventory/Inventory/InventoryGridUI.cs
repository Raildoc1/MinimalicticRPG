using KG.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.UI
{
    public class InventoryGridUI : MonoBehaviour
    {

        public PlayerInventory inventory;

        private InventorySlotUI[] slots;

        private void Awake()
        {

            slots = GetComponentsInChildren<InventorySlotUI>();

            int i = 0;

            foreach (var slot in slots)
            {
                slot.index = i++;
            }

        }

        private void OnEnable()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            for (int i = 0; i < Mathf.Min(slots.Length, inventory.items.Count); i++)
            {
                slots[i].image.sprite = inventory.items[i].item.icon;
                slots[i].image.enabled = true;
            }

            for (int i = inventory.items.Count; i < slots.Length; i++)
            {
                slots[i].image.enabled = false;
            }
        }

    }
}
