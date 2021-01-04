using KG.Core;
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

        private PlayerInventory playerInventory;
        private bool inventoryOpened = false;

        private void Awake()
        {
            playerInventory = GetComponent<PlayerInventory>();
            inventory.SetActive(false);
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

