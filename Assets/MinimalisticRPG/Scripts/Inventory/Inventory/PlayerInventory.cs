using PixelCrushers.DialogueSystem;

namespace KG.Inventory {
    public class PlayerInventory : ItemCollection {

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
