using KG.Core;
using KG.Inventory;
using KG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace KG.CombatCore
{
    [RequireComponent(typeof(Animator), typeof(StateSwitch))]
    public class Combat : MonoBehaviour
    {
        [SerializeField] private List<string> _tagsToAttack;
        [SerializeField] private Collider _rightArmHitbox;
        [SerializeField] private Collider _leftArmHitbox;

        private AnimatorProxy _animatorProxy;
        private StateSwitch _stateSwitch;
        private Collider _weaponHitBox;
        private StatsHolder _statsHolder;
        private Item _currentWeapon = null;

        public void SetCurrentWeapon(Item weapon)
        {
            _currentWeapon = weapon;
        }

        public Collider WeaponHitBox
        {
            get
            {
                return _weaponHitBox;
            }
            set
            {
                _weaponHitBox = value;
            }
        }


        public bool IsTagRight(string tag)
        {
            foreach (var t in _tagsToAttack)
            {
                if (t.Equals(tag)) return true;
            }
            return false;
        }

        private void Start()
        {
            _animatorProxy = GetComponent<AnimatorProxy>();
            _stateSwitch = GetComponent<StateSwitch>();
            _statsHolder = GetComponent<StatsHolder>();

            if (_rightArmHitbox)
            {
                _rightArmHitbox.GetComponent<HitBox>().SetOwner(this);
            }

            if (_leftArmHitbox)
            {
                _leftArmHitbox.GetComponent<HitBox>().SetOwner(this);
            }

        }

        public void Attack()
        {
            if (_stateSwitch.CurrentState != State.COMBAT)
            {
                return;
            }

            if (_statsHolder.Stamina <= 0.01f)
            {
                return;
            }

            _animatorProxy.Attack();
        }

        public void SendDamage()
        {

        }

        public void StartDamage(bool rightArm = true)
        {
            _statsHolder.Stamina -= _currentWeapon.GetAttributeValue(AttributeType.STAMINA_COST);
            if (WeaponHitBox)
            {
                WeaponHitBox.enabled = true;
            }
            else
            {
                if (rightArm)
                {
                    _rightArmHitbox.enabled = true;
                }
                else
                {
                    _leftArmHitbox.enabled = true;
                }
            }
        }

        public void EndDamage(bool rightArm = true)
        {
            if (WeaponHitBox)
            {
                WeaponHitBox.enabled = false;
            }
            else
            {
                if (rightArm)
                {
                    _rightArmHitbox.enabled = false;
                }
                else
                {
                    _leftArmHitbox.enabled = false;
                }
            }
        }

        public void Dodge()
        {
            if (_stateSwitch.CurrentState != State.COMBAT)
            {
                return;
            }

            _animatorProxy.Dodge();
        }
    }
}
