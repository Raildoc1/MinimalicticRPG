using KG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace KG.CombatCore
{
    [RequireComponent(typeof(Animator), typeof(StateSwitch))]
    public class Combat : MonoBehaviour
    {

        [SerializeField] private List<string> _tagsToAttack;
        [SerializeField] private Collider rightArmHitbox;
        [SerializeField] private Collider leftArmHitbox;

        private AnimatorProxy animatorProxy;
        private StateSwitch stateSwitch;
        private Collider _weaponHitBox;


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
            animatorProxy = GetComponent<AnimatorProxy>();
            stateSwitch = GetComponent<StateSwitch>();

            rightArmHitbox.GetComponent<HitBox>().SetOwner(this);
            leftArmHitbox.GetComponent<HitBox>().SetOwner(this);
        }

        public void Attack()
        {
            if (stateSwitch.CurrentState != State.COMBAT)
            {
                return;
            }
            animatorProxy.Attack();
        }

        public void SendDamage()
        {
            //Debug.Log($"{name} sending damage");
        }

        public void StartDamage(bool rightArm = true)
        {
            if (WeaponHitBox)
            {
                WeaponHitBox.enabled = true;
            }
            else
            {
                if (rightArm)
                {
                    rightArmHitbox.enabled = true;
                }
                else
                {
                    leftArmHitbox.enabled = true;
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
                    rightArmHitbox.enabled = false;
                }
                else
                {
                    leftArmHitbox.enabled = false;
                }
            }
        }

        public void Dodge()
        {
            if (stateSwitch.CurrentState != State.COMBAT)
            {
                return;
            }

            animatorProxy.Dodge();
        }
    }
}
