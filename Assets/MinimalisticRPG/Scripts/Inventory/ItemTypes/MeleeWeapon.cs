using UnityEngine;

namespace KG.Inventory
{
    [System.Serializable]
    public class MeleeWeapon : Weapon
    {
        public int requiredStrength;
        public GameObject prefab;
    }
}
