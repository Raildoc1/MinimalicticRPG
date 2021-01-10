using KG.Core;
using KG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(InputHandler))]
    public class InventoryView : MonoBehaviour
    {

        public GameObject inventory;
        public StateSwitch playerStateSwitch;

        private PlayerInventory playerInventory;
        private InputHandler inputHandler;
        private bool inventoryOpened = false;
        private float lockInputTime = .75f;

        private void Awake()
        {
            playerInventory = GetComponent<PlayerInventory>();
            inputHandler = GetComponent<InputHandler>();
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
            }

        }

        public void OpenInventory()
        {
            UnlockCursor();
            inventory.SetActive(true);
            playerStateSwitch.SetCurrentState(State.INVENTORY);
            inventoryOpened = true;
        }

        private static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        private static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void CloseInventory()
        {
            inputHandler.LockInputAxisInput(lockInputTime);
            LockCursor();
            inventory.SetActive(false);
            playerStateSwitch.SetCurrentState(State.PEACE);
            inventoryOpened = false;
        }

    }
}

