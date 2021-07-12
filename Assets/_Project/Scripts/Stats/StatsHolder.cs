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
        private float _currentHealth;
        private float _currentStamina;
        private float _currentPoise;
        private bool _staminaRecoveryStopped = false;
        private bool _poiseRecoveryStopped = false;
        private Coroutine _staminaRecoveryRoutine = null;
        private Coroutine _poiseRecoveryRoutine = null;

        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _maxStamina = 100;
        [SerializeField] private int _maxPoise = 100;
        [SerializeField] private int _strength = 10;
        [SerializeField] private int _dexterity = 10;
        [SerializeField] private int _intelligence = 10;
        [SerializeField] private float _staminaRecoverySpeed = 1;

        private const float StaminaRecoveryDelay = 1f;

        protected AnimatorProxy Animator;
        protected StateSwitch StateSwitch;

        public UnityEvent<Transform> OnGetDamage = new UnityEvent<Transform>();

        public delegate void OnHealthUpdateEvent(float current, int max);
        public event OnHealthUpdateEvent OnHealthUpdate;

        public delegate void OnStaminaUpdateEvent(float current, int max);
        public event OnStaminaUpdateEvent OnStaminaUpdate;

        public delegate void OnPoiseUpdateEvent(float current, int max);
        public event OnPoiseUpdateEvent OnPoiseUpdate;

        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
        }

        public float Health
        {
            get
            {
                return _currentHealth;
            }
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth == 0)
                {
                    IsDead = true;
                    StateSwitch.CurrentState = State.DEAD;
                }
                OnHealthUpdate?.Invoke(Health, MaxHealth);
            }
        }

        public float Stamina
        {
            get => _currentStamina;
            set 
            {
                var clampedValue = Mathf.Clamp(value, 0, _maxStamina);

                if (clampedValue != _currentStamina)
                {
                    OnStaminaUpdate?.Invoke(clampedValue, _maxStamina);
                }

                if (clampedValue < _currentStamina)
                {
                    StopStaminaRecoveryFor(StaminaRecoveryDelay);
                }

                _currentStamina = clampedValue;
            }
        }

        public float Poise
        {
            get => _currentPoise;
            set 
            {
                var clampedValue = Mathf.Clamp(value, 0, _maxPoise);

                if (clampedValue != _currentPoise)
                {
                    OnStaminaUpdate?.Invoke(clampedValue, _maxPoise);
                }

                if (clampedValue < _currentPoise)
                {
                    StopStaminaRecoveryFor(StaminaRecoveryDelay);
                }

                _currentPoise = clampedValue;
            }
        }

        public int Strength
        {
            get
            {
                return _strength;
            }
        }

        public int Dexterity
        {
            get
            {
                return _dexterity;
            }
        }

        public int Intelligence
        {
            get
            {
                return _intelligence;
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

        protected AnimatorProxy animatorProxy
        {
            get
            {
                if (!Animator) Animator = GetComponent<AnimatorProxy>();
                return Animator;
            }
        }

        protected virtual void Awake()
        {
            _currentHealth = _maxHealth;
            _currentStamina = _maxStamina;
            StateSwitch = GetComponent<StateSwitch>();
        }

        private void Update()
        {
            if (!_staminaRecoveryStopped)
            {
                Stamina += _staminaRecoverySpeed * Time.deltaTime;
            }

            if (!_poiseRecoveryStopped)
            {
                Poise += _staminaRecoverySpeed * Time.deltaTime;
            }
        }

        public void Eat(Item item)
        {
            if (!item.consumable)
            {
                return;
            }

            Health += item.GetAttributeValue(AttributeType.HEALTH_RECOVERY);
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

        public void StopStaminaRecoveryFor(float time)
        {
            if (_staminaRecoveryRoutine != null)
            {
                StopCoroutine(_staminaRecoveryRoutine);
            }
            _staminaRecoveryRoutine = StartCoroutine(StopStaminaRecoveryRoutine(time));
        }

        private IEnumerator StopStaminaRecoveryRoutine(float time)
        {
            _staminaRecoveryStopped = true;
            yield return new WaitForSeconds(time);
            _staminaRecoveryStopped = false;
        }

        public void StopPoiseRecoveryFor(float time)
        {
            if (_staminaRecoveryRoutine != null)
            {
                StopCoroutine(_poiseRecoveryRoutine);
            }
            _poiseRecoveryRoutine = StartCoroutine(StopPoiseRecoveryRoutine(time));
        }

        private IEnumerator StopPoiseRecoveryRoutine(float time)
        {
            _poiseRecoveryStopped = true;
            yield return new WaitForSeconds(time);
            _poiseRecoveryStopped = false;
        }
    }
}


