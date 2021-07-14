﻿namespace KG.Inventory{
    public enum DamageType {
        PHYS,
        MAGIC,
        FIRE,
        ICE
    }
    [System.Serializable]
    public class ItemDamage {
        public int poiseDrain;
        public int value;
        public DamageType damageType;
    }
}