using KG.Core;
using KG.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KG.Stats
{
    [RequireComponent(typeof(AnimatorProxy))]
    [RequireComponent(typeof(StateSwitch))]
    public class StatsHolder : MonoBehaviour
    {

        public UnityEvent<int, int> OnHealthUpdate = new UnityEvent<int, int>();
        public UnityEvent<Transform> OnGetDamage = new UnityEvent<Transform>();

        #region SerializableAndPublicFields

        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected int strength = 10;
        [SerializeField] protected int dexterity = 10;
        [SerializeField] protected int intelligence = 10;

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
                    stateSwitch.CurrentState = State.DEAD;
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

        protected int _currentHealth;
        protected AnimatorProxy _animator;
        protected StateSwitch stateSwitch;

        protected AnimatorProxy animatorProxy
        {
            get
            {
                if (!_animator) _animator = GetComponent<AnimatorProxy>();
                return _animator;
            }
        }

        #endregion

        protected virtual void Awake()
        {
            _currentHealth = maxHealth;
            stateSwitch = GetComponent<StateSwitch>();
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

        public void ApplyPhysicalDamage(int damage, Transform target = null)
        {
            Health -= damage < 5 ? 5 : damage;
            animatorProxy.GetDamage();

            if (target)
            {
                OnGetDamage.Invoke(target);
            }

        }

    }
}


