using System.Collections;
using KG.Core;
using UnityEngine;

namespace KG.Inventory {
    [RequireComponent(typeof(StateSwitch), typeof(Animator), typeof(EquipmentDisplay))]
    public class Equipment : MonoBehaviour {
        
        [SerializeField] private bool DEBUG_MODE = false;

        public enum WeaponType { NO_WEAPON = 0, MELEE = 1, BOW = 2, MAGIC = 3 }

        [SerializeField] private MeleeWeapon _meleeWeapon;
        private Animator animator;
        private StateSwitch stateSwitch;
        private EquipmentDisplay equipmentDisplay;

        private WeaponType lastWeapon = WeaponType.MELEE;
        private WeaponType currentWeapon = WeaponType.NO_WEAPON;

        public MeleeWeapon MeleeWeapon {
            get { 
                return _meleeWeapon;
            }
        }
        
        public bool IsEquiping {
            get {
                if(!animator) return false;
                return animator.GetBool("IsEquiping");
            }
            set {
                if(animator) animator.SetBool("IsEquiping", value);
            }
        }

        public bool IsUnequiping {
            get {
                if(!animator) return false;
                return animator.GetBool("IsUnequiping");
            }
            set {
                if(animator) animator.SetBool("IsUnequiping", value);
            }
        }

        private void Start() {
            animator = GetComponent<Animator>();
            stateSwitch = GetComponent<StateSwitch>();
            equipmentDisplay = GetComponent<EquipmentDisplay>();
            equipmentDisplay.Unequip(_meleeWeapon);
        }

        public void SetWeapon(string weaponName) {
            MeleeWeapon weapon = ItemDatabase.Instance.GetWeapon(weaponName);
            if(!weapon) {
                Debug.LogWarning($"Weapon {weaponName} not found! Please, add it to \"Resources/Items/Items/\".");
            }
            _meleeWeapon = weapon;
        }

        public void DrawHideWeapon() {
            DrawHideWeapon(lastWeapon);
        }
        public void DrawHideWeapon(WeaponType weaponType) {
            if(weaponType == currentWeapon) StartCoroutine(HideWeaponRoutine());
            else {
                StartCoroutine(DrawWeaponRoutine(weaponType));
            }
        }

        public void DrawWeapon() {
            StartCoroutine(DrawWeaponRoutine(lastWeapon));
        }

        public void HideWeapon() {
            StartCoroutine(HideWeaponRoutine());
        }

        private IEnumerator HideWeaponRoutine() {
            yield return Hide(currentWeapon);
            currentWeapon = WeaponType.NO_WEAPON;
        }

        private IEnumerator DrawWeaponRoutine(WeaponType weaponType) {
            lastWeapon = weaponType;
            yield return Draw(lastWeapon);
            currentWeapon = weaponType;
        }

        private IEnumerator Draw(WeaponType weaponType) {
            if(DEBUG_MODE) Debug.Log($"Drawing {weaponType}...");
            animator.SetTrigger("Equip");
            IsEquiping = true;
            while (IsEquiping) {
                yield return null;
            }
            if (DEBUG_MODE) Debug.Log($"Drawing IsEquiping = {IsEquiping}.");
            equipmentDisplay.Equip(_meleeWeapon);
            SetState(State.COMBAT);
            if(DEBUG_MODE) Debug.Log($"{weaponType} is drawn successfully!");
        }

        private IEnumerator Hide(WeaponType weaponType) {
            if(DEBUG_MODE) Debug.Log($"Hiding {weaponType}...");
            animator.SetTrigger("Unequip");
            IsUnequiping = true;
            while (IsUnequiping) {
                yield return null;
            }
            if (DEBUG_MODE) Debug.Log($"Hiding IsUnequiping = {IsUnequiping}.");
            equipmentDisplay.Unequip(_meleeWeapon);
            SetState(State.PEACE);
            if(DEBUG_MODE) Debug.Log($"{weaponType} is hidden successfully!");
        }

        private void SetState(State state) {
            stateSwitch.CurrentState = state;
        }
    }
}

