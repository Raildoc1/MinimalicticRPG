using KG.Inventory;
using KG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KG.UI
{
    public class InventoryGridUI : MonoBehaviour
    {

        public UnityEvent OnUpdateUI = new UnityEvent();

        public PlayerInventory inventory;
        public Equipment equipment;
        public StatsHolder statsHolder;

        private InventorySlotUI[] slots;

        private void Awake()
        {

            slots = GetComponentsInChildren<InventorySlotUI>();

            int i = 0;

            foreach (var slot in slots)
            {
                slot.index = i++;
                slot.inventoryGridUI = this;
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
                slots[i].equipedIcon.SetActive(inventory.items[i].isEquiped && inventory.items[i].item.canBeEquiped);
            }

            for (int i = inventory.items.Count; i < slots.Length; i++)
            {
                slots[i].image.enabled = false;
                slots[i].equipedIcon.SetActive(false);
            }

            OnUpdateUI.Invoke();
        }

        public void OnClick(int index)
        {

            var item = inventory.GetItemByIndex(index);

            if (item == null)
            {
                Debug.LogError($"No item at index {index}!");
            }
            else if (item.canBeEquiped)
            {
                equipment.EquipUnequip(item);
            }
            else if (item.consumable)
            {
                statsHolder.Eat(item);
                inventory.RemoveItems(item.itemName);
            }

            UpdateUI();

        }

    }
}

