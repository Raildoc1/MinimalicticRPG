using KG.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KG.Stats
{
    [RequireComponent(typeof(AnimatorProxy))]
    public class StatsHolder : MonoBehaviour
    {

        public UnityEvent<int, int> OnHealthUpdate = new UnityEvent<int, int>();

        #region SerializableAndPublicFields

        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int strength = 10;
        [SerializeField] private int dexterity = 10;
        [SerializeField] private int intelligence = 10;

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
        }

        public int Health
        {
            get
            {
                return _currentHealth;
            }
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                if (_currentHealth == 0)
                {
                    IsDead = true;
                }
                OnHealthUpdate.Invoke(Health, MaxHealth);
            }
        }

        public int Strength
        {
            get
            {
                return strength;
            }
        }

        public int Dexterity
        {
            get
            {
                return dexterity;
            }
        }

        public int Intelligence
        {
            get
            {
                return intelligence;
            }
        }

        public bool IsDead
        {
            get
            {
                return animatorProxy.isDead;
            }
            set
            {
                animatorProxy.isDead = value;
            }
        }

        #endregion

        #region PrivateFields

        private int _currentHealth;
        private AnimatorProxy _animator;

        private AnimatorProxy animatorProxy
        {
            get
            {
                if (!_animator) _animator = GetComponent<AnimatorProxy>();
                return _animator;
            }
        }

        #endregion

        private void Start()
        {
            _currentHealth = maxHealth;
        }


        public void Eat(Item item)
        {
            if (!item.consumable)
            {
                return;
            }

            Health += item.GetAttributeValue(AttributeType.HALTH_RECOVERY);
        }

        public void ApplyDamage(List<ItemDamage> damage)
        {
            var sum = 0;
            foreach (var d in damage)
            {
                sum += d.value;
            }
            if (sum <= 5) sum = 5;
            Health -= sum;
            animatorProxy.GetDamage();
        }

        public void ApplyPhysicalDamage(int damage)
        {
            Health -= damage < 5 ? 5 : damage;
            animatorProxy.GetDamage();
        }

    }
}


