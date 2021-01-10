using System.Collections;
using System.Collections.Generic;
using KG.CombatCore;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(Combat))]
    public class EquipmentDisplay : MonoBehaviour
    {

        [SerializeField] private Transform _rightArmMeleeHolder;
        [SerializeField] private Transform _beltMeleeHolder;

        private GameObject _rightArmMeleeObject = null;
        private GameObject _beltMeleeObject = null;
        private Combat combat;

        private void Awake()
        {
            combat = GetComponent<Combat>();
        }

        public void Equip(Item weapon)
        {

            if (weapon == null)
            {
                return;
            }

            if (weapon.type == ItemType.MELEE_WEAPON)
            {
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
                _rightArmMeleeObject = Instantiate(weapon.gameObject, _rightArmMeleeHolder);
                var hitboxCollider = _rightArmMeleeObject.GetComponentInChildren<Collider>();
                var hitbox = _rightArmMeleeObject.GetComponentInChildren<HitBox>();
                hitbox.SetOwner(combat);
                //hitbox.SetWeaponName(weapon.name);
                combat.WeaponHitBox = hitboxCollider;
            }
        }

        public void Hide(Item weapon)
        {

            if (weapon == null)
            {
                return;
            }

            if (weapon.type == ItemType.MELEE_WEAPON)
            {
                combat.WeaponHitBox = null;
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
                _beltMeleeObject = Instantiate(weapon.gameObject, _beltMeleeHolder);
            }
        }

        public void Unequip(Item weapon)
        {
            if (weapon.type == ItemType.MELEE_WEAPON)
            {
                combat.WeaponHitBox = null;
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
            }
        }

    }
}