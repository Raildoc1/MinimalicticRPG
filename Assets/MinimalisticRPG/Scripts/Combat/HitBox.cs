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

        private Combat owner = null;
        [SerializeField] private string weaponName;
        private MeleeWeapon weapon;

        private void Start()
        {
            weapon = PlayerInventory.instance.FindItemByName(weaponName) as MeleeWeapon;

            if (weapon == null)
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

            target.ApplyDamage(weapon.damage);

            GetComponent<Collider>().enabled = false;
        }

    }
}
