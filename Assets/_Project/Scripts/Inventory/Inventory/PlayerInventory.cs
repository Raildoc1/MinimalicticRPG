using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Inventory
{
    public class PlayerInventory : ItemCollection
    {

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
        }

        #region Lua

        private void OnEnable()
        {
            Lua.RegisterFunction("EarnGold", this, SymbolExtensions.GetMethodInfo(() => EarnGold((double)0)));
            Lua.RegisterFunction("SpendGold", this, SymbolExtensions.GetMethodInfo(() => SpendGold((double)0)));
            Lua.RegisterFunction("HasItems", this, SymbolExtensions.GetMethodInfo(() => HasItems(string.Empty, (double)0)));
        }

        private void OnDisable()
        {
            Lua.UnregisterFunction("EarnGold");
            Lua.UnregisterFunction("SpendGold");
            Lua.UnregisterFunction("HasItems");
        }

        #endregion

    }
}
