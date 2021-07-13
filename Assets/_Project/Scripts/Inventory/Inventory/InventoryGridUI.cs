using KG.Inventory;
using KG.Stats;
using UnityEngine;

namespace KG.UI
{
    public class InventoryGridUI : MonoBehaviour
    {
        private PlayerInventory _inventory;
        private Equipment _equipment;
        private StatsHolder _statsHolder;

        private InventorySlotUI[] _slots;

        public delegate void OnUpdateUIEvent();
        public event OnUpdateUIEvent OnUpdateUI;

        private void Awake()
        {
            _slots = GetComponentsInChildren<InventorySlotUI>();

            var player = GameObject.FindGameObjectWithTag("Player");

            _inventory = player.GetComponent<PlayerInventory>();
            _equipment = player.GetComponent<Equipment>();
            _statsHolder = player.GetComponent<StatsHolder>();

            int i = 0;

            foreach (var slot in _slots)
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
            for (int i = 0; i < Mathf.Min(_slots.Length, _inventory.items.Count); i++)
            {
                _slots[i].image.sprite = _inventory.items[i].item.icon;
                _slots[i].image.enabled = true;
                _slots[i].equipedIcon.SetActive(_inventory.items[i].isEquiped && _inventory.items[i].item.canBeEquiped);
            }

            for (int i = _inventory.items.Count; i < _slots.Length; i++)
            {
                _slots[i].image.enabled = false;
                _slots[i].equipedIcon.SetActive(false);
            }

            OnUpdateUI?.Invoke();
        }

        public void OnClick(int index)
        {
            if (index >= _inventory.items.Count)
            {
                return;
            }

            var item = _inventory.GetItemByIndex(index);

            if (item == null)
            {
                Debug.LogError($"No item at index {index}!");
            }
            else if (item.canBeEquiped)
            {
                _equipment.EquipUnequip(item);
            }
            else if (item.consumable)
            {
                _statsHolder.Eat(item);
                _inventory.RemoveItems(item.itemName);
            }

            UpdateUI();
        }

    }
}

