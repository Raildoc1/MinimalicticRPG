using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace KG.Inventory {
    public class PlayerInventory : ItemCollection {


        private void Start()
        {
            Debug.Log($"hash {Animator.StringToHash("ITM_PICKAXE")}");
        }

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
    }
}
