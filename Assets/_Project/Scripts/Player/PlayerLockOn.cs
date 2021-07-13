using Cinemachine;
using KG.CameraControl;
using KG.Core;
using KG.Interact;
using KG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.CombatCore
{
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(StateSwitch))]
    [RequireComponent(typeof(PlayerTargetDetector))]
    public class PlayerLockOn : MonoBehaviour
    {
        private Transform _currentTarget = null;
        private bool _isLockedOn = false;
        private PlayerMover _mover;
        private StateSwitch _stateSwitch;
        private PlayerTargetDetector _playerTargetDetector;
        private InputHandler _inputHandler;

        [SerializeField] private CameraStateController cameraStateController;
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private float targetWeight = 1.5f;
        [SerializeField] private float targetRadius = 1f;
        [SerializeField] private List<string> tagsToLockOn;
        [SerializeField] private float distanceToDetect = 10f;
        [SerializeField] private float maxAngle = 90f;

        private void Awake()
        {
            _mover = GetComponent<PlayerMover>();
            _playerTargetDetector = GetComponent<PlayerTargetDetector>();
        }

        private void OnEnable()
        {
            _stateSwitch = GetComponent<StateSwitch>();
            _inputHandler = FindObjectOfType<InputHandler>();
            _inputHandler.OnLockOnKeyInput += LockOn;
            _stateSwitch.OnStateChange += OnChangeState;
        }

        private void OnDisable()
        {
            _inputHandler.OnLockOnKeyInput -= LockOn;
            _stateSwitch.OnStateChange -= OnChangeState;
        }

        private void SetTarget(Transform transform)
        {

            if (_currentTarget)
            {
                targetGroup.RemoveMember(_currentTarget);
            }

            if (transform)
            {
                var interactable = transform.GetComponent<Interactable>();

                if (interactable)
                {
                    _playerTargetDetector.ForceNewTarget(interactable);
                }

                targetGroup.AddMember(transform, targetWeight, targetRadius);
                cameraStateController.LockOnTarget();
                _isLockedOn = true;
            }
            else
            {
                _playerTargetDetector.UnlockTarget();
                cameraStateController.UnlockTarget();
                _isLockedOn = false;
            }
            _currentTarget = transform;
            _mover.IsStrafing = _isLockedOn;
            _mover.LookAt = transform;
        }

        public void LockOn()
        {
            if (_stateSwitch.CurrentState != State.COMBAT)
            {
                ResetTarget();
                return;
            }

            if (_isLockedOn)
            {
                ResetTarget();
            }
            else
            {
                SetTarget(FindBestTarget());
            }
        }

        public void ResetTarget()
        {
            SetTarget(null);
        }

        public void ResetTarget(State currentState)
        {
            _playerTargetDetector.UnlockTarget();
            cameraStateController.UnlockTarget(currentState);
            _isLockedOn = false;
            _mover.IsStrafing = false;
            _currentTarget = null;
            _mover.LookAt = null;
        }

        public void OnChangeState(State _, State currentState)
        {
            if (currentState != State.COMBAT)
            {
                ResetTarget(currentState);
            }
        }

        private Transform FindBestTarget()
        {

            Transform target = null;
            float absTargetAngle = 360f;

            foreach (var tag in tagsToLockOn)
            {
                foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) > distanceToDetect) continue;
                    float angle = AbsTargetAngle(obj.transform);
                    //Debug.Log($"Angle = {angle}");
                    if (angle < absTargetAngle)
                    {
                        target = obj.transform;
                        absTargetAngle = angle;
                    }
                }
            }

            if (absTargetAngle > maxAngle) return null;

            return target;
        }

        private float AbsTargetAngle(Transform target)
        {
            var cameraTransform = Camera.main.transform;
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            Vector3 targetDirection = target.transform.position - cameraTransform.position;
            targetDirection.y = 0;
            return Mathf.Abs(Vector3.Angle(cameraForward, targetDirection));
        }
        /*
        private void Update() {
            var cameraTransform = Camera.main.transform;
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward);
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            Debug.DrawRay(Camera.main.transform.position, cameraForward);
        }*/
    }
}
