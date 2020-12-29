using KG.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Stats {
    [RequireComponent(typeof(Animator))]
    public class StatsHolder : MonoBehaviour {

        #region SerializableAndPublicFields

        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int strength = 10;
        [SerializeField] private int dexterity = 10;
        [SerializeField] private int intelligence = 10;

        public int Health {
            get {
                return _currentHealth;
            }
            set {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                if (_currentHealth == 0) IsDead = true;
            }
        }

        public bool IsDead {
            get {
                return animator.GetBool("IsDead");
            }
            set {
                animator.SetBool("IsDead", value);
            }
        }
        #endregion

        #region PrivateFields

        private int _currentHealth;
        private Animator _animator;

        private Animator animator{
            get {
                if (!_animator) _animator = GetComponent<Animator>();
                return _animator;
            }
        }

        #endregion

        private void Start() {
            _currentHealth = maxHealth;
        }

        public void ApplyDamage(List<ItemDamage> damage) {
            var sum = 0;
            foreach (var d in damage) {
                sum += d.value;
            }
            if (sum <= 5) sum = 5;
            Health -= sum;
            animator.SetTrigger("GetDamage");
            Debug.Log("GetDamage");
        }

    }
}


