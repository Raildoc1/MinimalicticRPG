using System.Collections.Generic;

namespace KG.Inventory
{
    [System.Serializable]
    public abstract class Weapon : Item
    {
        public List<ItemDamage> damage;
    }
}
