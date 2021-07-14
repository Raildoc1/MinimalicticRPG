﻿using KG.Core;
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
        private bool _inDodge = false;
        private Coroutine _staminaRecoveryRoutine = null;
        private Coroutine _poiseRecoveryRoutine = null;
        private Coroutine _dodgeRoutine = null;

        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _maxStamina = 100;
        [SerializeField] private int _maxPoise = 100;
        [SerializeField] private int _strength = 10;
        [SerializeField] private int _dexterity = 10;
        [SerializeField] private int _intelligence = 10;
        [SerializeField] private float _staminaRecoverySpeed = 1;
        [SerializeField] private float _dodgeIgnoreDamageTime = 0.5f;

        private const float StaminaRecoveryDelay = 1f;

        private AnimatorProxy _animator;
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
                Debug.Log($"Health: gameObject = {gameObject}");
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
                    OnPoiseUpdate?.Invoke(clampedValue, _maxPoise);
                }

                if (clampedValue < _currentPoise)
                {
                    StopPoiseRecoveryFor(StaminaRecoveryDelay);
                }

                _currentPoise = clampedValue;
                AnimatorProxy.Poise = Mathf.RoundToInt(_currentPoise);
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
                return AnimatorProxy.isDead;
            }
            set
            {
                AnimatorProxy.isDead = value;
            }
        }

        protected AnimatorProxy AnimatorProxy
        {
            get
            {
                if (!_animator) _animator = GetComponent<AnimatorProxy>();
                return _animator;
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
            if (_inDodge)
            {
                return;
            }

            var healthDelta = 0;
            var poiseDelta = 0;

            foreach (var d in damage)
            {
                healthDelta += d.value;
                poiseDelta += d.poiseDrain;
            }

            if (healthDelta <= 5)
            {
                healthDelta = 5;
            }

            Health -= healthDelta;
            Poise -= poiseDelta;
            AnimatorProxy.GetDamage();
        }

        public void ApplyPhysicalDamage(int damage, int poise, Transform target = null)
        {
            if (_inDodge)
            {
                return;
            }

            Health -= damage < 5 ? 5 : damage;
            AnimatorProxy.GetDamage();
            Poise -= poise;

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
            if (_poiseRecoveryRoutine != null)
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

        public void Dodge()
        {
            Debug.Log("Dodge");
            if (_dodgeRoutine != null)
            {
                StopCoroutine(_dodgeRoutine);
            }
            _dodgeRoutine = StartCoroutine(DodgeRoutine(_dodgeIgnoreDamageTime));
        }

        private IEnumerator DodgeRoutine(float time)
        {
            _inDodge = true;
            yield return new WaitForSeconds(time);
            _inDodge = false;
        }
    }
}

