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

        public void UpdateUI()
        {
            for (int i = 0; i < Mathf.Min(slots.Length, inventory.items.Count); i++)
            {
                slots[i].image.sprite = inventory.items[i].item.icon;
                slots[i].image.enabled = true;
            }

            var empty_slots = slots.Length - inventory.items.Count + 1;

            for (int i = inventory.items.Count; i < empty_slots; i++)
            {
                slots[i].image.enabled = false;
            }
        }

    }
}

