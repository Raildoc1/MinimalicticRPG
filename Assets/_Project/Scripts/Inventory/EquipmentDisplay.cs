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
        private Combat _combat;

        private void Awake()
        {
            _combat = GetComponent<Combat>();
        }

        public void Equip(Item weapon)
        {
            if (weapon == null)
            {
                return;
            }

            _combat.SetCurrentWeapon(weapon);

            if (weapon.type == ItemType.MELEE_WEAPON)
            {
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
                _rightArmMeleeObject = Instantiate(weapon.gameObject, _rightArmMeleeHolder);
                var hitboxCollider = _rightArmMeleeObject.GetComponentInChildren<Collider>();
                var hitbox = _rightArmMeleeObject.GetComponentInChildren<HitBox>();
                hitbox.SetOwner(_combat);
                //hitbox.SetWeaponName(weapon.name);
                _combat.WeaponHitBox = hitboxCollider;
            }
        }

        public void Hide(Item weapon)
        {
            if (weapon == null)
            {
                return;
            }

            _combat.SetCurrentWeapon(null);

            if (weapon.type == ItemType.MELEE_WEAPON)
            {
                _combat.WeaponHitBox = null;
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
                _beltMeleeObject = Instantiate(weapon.gameObject, _beltMeleeHolder);
            }
        }

        public void Unequip(Item weapon)
        {
            _combat.SetCurrentWeapon(null);

            if (weapon.type == ItemType.MELEE_WEAPON)
            {
                _combat.WeaponHitBox = null;
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
            }
        }

    }
}