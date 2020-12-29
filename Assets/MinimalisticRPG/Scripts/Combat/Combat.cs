using KG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace KG.CombatCore
{
    [RequireComponent(typeof(Animator), typeof(StateSwitch))]
    public class Combat : MonoBehaviour
    {

        [SerializeField] private List<string> _tagsToAttack;

        private Animator animator;
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
            animator = GetComponent<Animator>();
            stateSwitch = GetComponent<StateSwitch>();
        }

        public void Attack()
        {
            if (stateSwitch.CurrentState != State.COMBAT) return;
            animator.SetTrigger("Attack");
        }

        public void SendDamage()
        {
            //Debug.Log($"{name} sending damage");
        }

        public void StartDamage()
        {
            if (WeaponHitBox) WeaponHitBox.enabled = true;
        }

        public void EndDamage()
        {
            if (WeaponHitBox) WeaponHitBox.enabled = false;
        }

    }
}
