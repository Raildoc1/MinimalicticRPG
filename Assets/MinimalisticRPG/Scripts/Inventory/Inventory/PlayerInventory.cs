using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace KG.Inventory {
    public class PlayerInventory : ItemCollection {

        #region Singleton

        public static PlayerInventory instance;

        private void InitSingleton()
        {
            if (instance)
            {
                Debug.LogError("More than one instance of PlayerInventory found!");
                Destroy(this);
                return;
            }

            instance = this;
        }

        #endregion

        private void Awake()
        {
            InitSingleton();

            if (!itemsList)
            {
                Debug.LogError("No ItemsList assigned in PlayerInventory!");
            }
        }

        public ItemsList itemsList;

        public Item FindItemByName(string name)
        {
            return FindItemByHash(Animator.StringToHash(name));
        }

        public Item FindItemByHash(int hash)
        {
            return itemsList.FindItemByHash(hash);
        }

        #region Lua

        private void OnEnable()
        {
            Lua.RegisterFunction("EarnGold", this, SymbolExtensions.GetMethodInfo(() => EarnGold((double)0)));
            Lua.RegisterFunction("SpendGold", this, SymbolExtensions.GetMethodInfo(() => SpendGold((double)0)));
        }

        private void OnDisable()
        {
            Lua.UnregisterFunction("EarnGold");
            Lua.UnregisterFunction("SpendGold");
        }

        #endregion

    }
}
