using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KG.Stats;
using KG.Inventory;

namespace KG.CombatCore
{
    [RequireComponent(typeof(Collider))]
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private string weaponName;

        private Combat owner = null;
        private Item weapon;

        private void Start()
        {
            weapon = PlayerInventory.instance.FindItemInDatabaseByName(weaponName);

            if (weapon == null || weapon.type != ItemType.MELEE_WEAPON)
            {
                Debug.LogWarning($"Cannot find weapon {weaponName}");
            }
        }

        public void SetOwner(Combat combat)
        {
            owner = combat;
        }

        public void SetWeaponName(string name)
        {
            weaponName = name;
        }

        private void OnTriggerStay(Collider other)
        {

            if (!owner) return;
            if (!owner.IsTagRight(other.gameObject.tag)) return;

            var target = other.gameObject.GetComponent<StatsHolder>();

            if (!target)
            {
                Debug.LogWarning($"Failed to send damage: {other.gameObject.name} doesn't have StatsHolder!");
                return;
            }

            target.ApplyPhysicalDamage(weapon.GetAttributeValue(AttributeType.PHYSICAL_DAMAGE), owner.transform);

            GetComponent<Collider>().enabled = false;
        }

    }
}
