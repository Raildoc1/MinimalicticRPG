using KG.Core;
using KG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(PlayerInventory))]
    public class InventoryView : MonoBehaviour
    {

        public GameObject inventory;
        public StateSwitch playerStateSwitch;
        public InventoryGridUI inventoryGridUI;

        private PlayerInventory playerInventory;
        private bool inventoryOpened = false;

        private void Awake()
        {
            playerInventory = GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            inventory.SetActive(inventoryOpened);
        }

        public void OpenCloseInventory()
        {
            if (inventoryOpened)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
                inventoryGridUI.UpdateUI();
            }

        }

        public void OpenInventory()
        {
            inventory.SetActive(true);
            playerStateSwitch.SetCurrentState(State.INVENTORY);
            inventoryOpened = true;
        }

        public void CloseInventory()
        {
            inventory.SetActive(false);
            playerStateSwitch.SetCurrentState(State.PEACE);
            inventoryOpened = false;
        }

    }
}

