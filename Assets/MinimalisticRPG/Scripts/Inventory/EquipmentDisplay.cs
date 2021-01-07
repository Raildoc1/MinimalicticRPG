using System.Collections;
using System.Collections.Generic;
using KG.CombatCore;
using UnityEngine;

namespace KG.Inventory {
    [RequireComponent(typeof(Combat))]
    public class EquipmentDisplay : MonoBehaviour {
        
        [SerializeField] private Transform _rightArmMeleeHolder;
        [SerializeField] private Transform _beltMeleeHolder;

        private GameObject _rightArmMeleeObject = null;
        private GameObject _beltMeleeObject = null;
        private Combat combat;

        private void Awake() {
            combat = GetComponent<Combat>();
        }

        public void Equip(Weapon weapon) {
            if(weapon is MeleeWeapon) {
                MeleeWeapon w = (MeleeWeapon)weapon;
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
                _rightArmMeleeObject = Instantiate(w.prefab, _rightArmMeleeHolder);
                var hitboxCollider = _rightArmMeleeObject.GetComponentInChildren<Collider>();
                var hitbox = _rightArmMeleeObject.GetComponentInChildren<HitBox>();
                hitbox.SetOwner(combat);
                //hitbox.SetWeaponName(weapon.name);
                combat.WeaponHitBox = hitboxCollider;
            }
        }

        public void Unequip(Weapon weapon) {
            if(weapon is MeleeWeapon) {
                MeleeWeapon w = (MeleeWeapon)weapon;
                combat.WeaponHitBox = null;
                Destroy(_rightArmMeleeObject);
                Destroy(_beltMeleeObject);
                _beltMeleeObject = Instantiate(w.prefab, _beltMeleeHolder);
            }
        }

    }
}