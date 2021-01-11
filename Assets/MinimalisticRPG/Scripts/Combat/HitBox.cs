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

        public int defaultDamage = 5;

        private Combat owner = null;
        private Item weapon;



        private void Start()
        {

            if (weaponName.Equals(""))
            {
                return;
            }

            weapon = PlayerInventory.instance.FindItemInDatabaseByName(weaponName);

            if (weapon == null || weapon.type != ItemType.MELEE_WEAPON)
            {
                Debug.LogWarning($"Cannot find weapon {weaponName}");
            }
        }

        public void SetOwner(Combat combat)
        {
            owner = combat;

            var col1 = GetComponent<Collider>();

            foreach (var col2 in combat.GetComponents<Collider>())
            {
                Physics.IgnoreCollision(col1, col2);
            }

        }

        public void SetWeaponName(string name)
        {
            weaponName = name;
        }

        private void OnTriggerStay(Collider other)
        {

            Debug.Log("You hitted some one!");

            if (!owner) return;

            Debug.Log($"He has tag {other.gameObject.tag}");
            if (!owner.IsTagRight(other.gameObject.tag)) return;

            Debug.Log("Have owner and tags are right!");

            var target = other.gameObject.GetComponent<StatsHolder>();

            if (!target)
            {
                Debug.LogWarning($"Failed to send damage: {other.gameObject.name} doesn't have StatsHolder!");
                return;
            }

            var damage = weaponName.Equals("") ? defaultDamage : weapon.GetAttributeValue(AttributeType.PHYSICAL_DAMAGE);

            target.ApplyPhysicalDamage(damage, owner.transform);

            GetComponent<Collider>().enabled = false;
        }

    }
}
