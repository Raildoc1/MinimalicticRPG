using KG.Core;
using KG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Interact
{
    [RequireComponent(typeof(PlayerStateSwitch))]
    public class PlayerTargetDetector : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private bool _fixedTarget = false;
        private PlayerStateSwitch _stateSwitch;

        [Header("References")]
        [SerializeField] private HealthBarView _enemyHpBar;

        [Header("Settings")]
        [SerializeField] private float _maxDistance = 6f;
        [SerializeField] private List<string> _tagsToDetect;

        public Interactable CurrentTarget { get; private set; } = null;

        public delegate void OnUpdateTargetEvent(Interactable target);
        public event OnUpdateTargetEvent OnUpdateTarget;

        private void Awake()
        {
            _stateSwitch = GetComponent<PlayerStateSwitch>();
        }

        private void OnEnable()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _inputHandler.OnMainKeyInput += Interact;
        }
        private void OnDisable()
        {
            _inputHandler.OnMainKeyInput -= Interact;
        }

        public void Interact()
        {
            if (_stateSwitch.CurrentState != State.PEACE || !CurrentTarget)
            {
                return;
            }

            if (CurrentTarget is Talkable)
            {
                _stateSwitch.CurrentState = State.DIALOG;
            }

            CurrentTarget.Interact(transform);
        }

        private void Update()
        {
            if (_fixedTarget) return;
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, _maxDistance);
            var new_target = GetNewTarget(hits);
            SetNewTarget(new_target);
        }

        public void ForceNewTarget(Interactable new_target)
        {
            if (!new_target)
            {
                Debug.LogError("ForceNewTarget: new_target cannot be null!");
            }

            _fixedTarget = true;

            SetNewTarget(new_target);
        }

        public void UnlockTarget()
        {
            _fixedTarget = false;
        }

        private void SetNewTarget(Interactable new_target)
        {
            if (new_target == null || new_target != CurrentTarget)
            {
                _enemyHpBar.Disable();
                OnUpdateTarget?.Invoke(new_target);
                CurrentTarget = new_target;
            }

            if (new_target)
            {
                var stats = new_target.GetComponent<StatsHolder>();

                if (stats)
                {
                    _enemyHpBar.Stats = stats;

                    _enemyHpBar.gameObject.SetActive(true);
                }
            }
        }

        private bool IsRightTag(string tag)
        {
            foreach (string s in _tagsToDetect)
            {
                if (string.Equals(tag, s)) return true;
            }
            return false;
        }

        private Interactable GetNewTarget(RaycastHit[] hits)
        {
            foreach (RaycastHit hit in hits)
            {
                Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
                if (i)
                    if (IsRightTag(i.tag))
                        return i;
            }
            return null;
        }
    }
}
