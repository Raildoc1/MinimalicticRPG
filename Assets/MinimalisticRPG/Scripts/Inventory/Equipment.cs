using System.Collections;
using KG.Core;
using UnityEngine;

namespace KG.Inventory
{
    [RequireComponent(typeof(AnimatorProxy))]
    [RequireComponent(typeof(EquipmentDisplay))]
    [RequireComponent(typeof(StateSwitch))]
    public class Equipment : MonoBehaviour
    {

        [SerializeField] private bool DEBUG_MODE = false;

        public enum WeaponType { NO_WEAPON = 0, MELEE = 1, BOW = 2, MAGIC = 3 }

        public string initWeaponName = "";

        private Item _meleeWeapon;
        private AnimatorProxy animatorProxy;
        private EquipmentDisplay equipmentDisplay;
        private StateSwitch stateSwitch;

        private WeaponType lastWeapon = WeaponType.MELEE;
        private WeaponType currentWeapon = WeaponType.NO_WEAPON;

        public Item MeleeWeapon
        {
            get
            {
                return _meleeWeapon;
            }
        }

        public bool IsEquiping
        {
            get
            {
                if (!animatorProxy) return false;
                return animatorProxy.isEquiping;
            }
            set
            {
                if (animatorProxy) animatorProxy.isEquiping = value;
            }
        }

        public bool IsUnequiping
        {
            get
            {
                if (!animatorProxy) return false;
                return animatorProxy.isUnequiping;
            }
            set
            {
                if (animatorProxy) animatorProxy.isUnequiping = value;
            }
        }

        private void Awake()
        {
            animatorProxy = GetComponent<AnimatorProxy>();
            equipmentDisplay = GetComponent<EquipmentDisplay>();
            stateSwitch = GetComponent<StateSwitch>();
        }

        private void Start()
        {

            stateSwitch.onStateChange.AddListener(OnStateChange);

            if (!initWeaponName.Equals(""))
            { 
                SetWeapon(initWeaponName);
                equipmentDisplay.Unequip(_meleeWeapon);
            }
        }

        private void OnDisable()
        {
            stateSwitch.onStateChange.RemoveListener(OnStateChange);
        }

        public void SetWeapon(string weaponName)
        {
            var weapon = PlayerInventory.instance.FindItemInDatabaseByName(weaponName);
            if (weapon == null)
            {
                Debug.LogWarning($"Weapon {weaponName} doesn't exists or {weaponName} is not a MeleeWeapon.");
            }
            _meleeWeapon = weapon;
        }

        public void DrawHideWeapon()
        {
            DrawHideWeapon(lastWeapon);
        }
        public void DrawHideWeapon(WeaponType weaponType)
        {
            if (weaponType == currentWeapon) StartCoroutine(HideWeaponRoutine());
            else
            {
                StartCoroutine(DrawWeaponRoutine(weaponType));
            }
        }

        public void DrawWeapon()
        {
            if (currentWeapon == lastWeapon)
            {
                return;
            }
            StartCoroutine(DrawWeaponRoutine(lastWeapon));
        }

        public void HideWeapon()
        {
            if (currentWeapon == WeaponType.NO_WEAPON)
            {
                return;
            }
            StartCoroutine(HideWeaponRoutine());
        }

        private IEnumerator HideWeaponRoutine()
        {
            yield return Hide(currentWeapon);
            currentWeapon = WeaponType.NO_WEAPON;
        }

        private IEnumerator DrawWeaponRoutine(WeaponType weaponType)
        {
            lastWeapon = weaponType;
            yield return Draw(lastWeapon);
            currentWeapon = weaponType;
        }

        private IEnumerator Draw(WeaponType weaponType)
        {
            if (DEBUG_MODE) Debug.Log($"Drawing {weaponType}...");
            animatorProxy.Equip();
            IsEquiping = true;
            while (IsEquiping)
            {
                yield return null;
            }
            if (DEBUG_MODE) Debug.Log($"Drawing IsEquiping = {IsEquiping}.");
            equipmentDisplay.Equip(_meleeWeapon);
            if (DEBUG_MODE) Debug.Log($"{weaponType} is drawn successfully!");
        }

        private IEnumerator Hide(WeaponType weaponType)
        {
            if (DEBUG_MODE) Debug.Log($"Hiding {weaponType}...");
            animatorProxy.Unequip();
            IsUnequiping = true;
            while (IsUnequiping)
            {
                yield return null;
            }
            if (DEBUG_MODE) Debug.Log($"Hiding IsUnequiping = {IsUnequiping}.");
            equipmentDisplay.Unequip(_meleeWeapon);
            if (DEBUG_MODE) Debug.Log($"{weaponType} is hidden successfully!");
        }

        public void OnStateChange(State prevState, State currentState)
        {
            if (prevState == State.COMBAT)
            {
                HideWeapon();
            }
            else if (currentState == State.COMBAT)
            {
                DrawWeapon();
            }
        }
    }
}

