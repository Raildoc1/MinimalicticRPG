using UnityEngine;

namespace KG.Inventory {
    [CreateAssetMenu(menuName = "Item/Weapon/Melee")]
    public class MeleeWeapon : Weapon {
        public int requiredStrength;
        public GameObject prefab;
    }
}
