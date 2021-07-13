using KG.Core;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(PlayerInventory))]
    public class InventoryView : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private PlayerStateSwitch _playerStateSwitch;
        private bool _inventoryOpened = false;
        private float _lockInputTime = .75f;

        public GameObject inventory;

        private void Start()
        {
            inventory.SetActive(_inventoryOpened);
            _playerStateSwitch = FindObjectOfType<PlayerStateSwitch>();
        }

        private void OnEnable()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _inputHandler.OnInventoryInput += OpenCloseInventory;
        }

        private void OnDisable()
        {
            _inputHandler.OnInventoryInput -= OpenCloseInventory;
        }

        public void OpenCloseInventory()
        {
            if (_inventoryOpened)
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
            _playerStateSwitch.SetCurrentState(State.INVENTORY);
            _inventoryOpened = true;
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
            _inputHandler.LockInputAxisInput(_lockInputTime);
            LockCursor();
            inventory.SetActive(false);
            _playerStateSwitch.SetCurrentState(State.PEACE);
            _inventoryOpened = false;
        }
    }
}

