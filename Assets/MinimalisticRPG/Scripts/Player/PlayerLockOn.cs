using Cinemachine;
using KG.CameraControl;
using KG.Core;
using KG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.CombatCore
{
    [RequireComponent(typeof(PlayerMover))]
    [RequireComponent(typeof(StateSwitch))]
    public class PlayerLockOn : MonoBehaviour
    {

        [SerializeField] private CameraStateController cameraStateController;

        [SerializeField] private CinemachineTargetGroup targetGroup;

        [SerializeField] private float targetWeight = 1.5f;
        [SerializeField] private float targetRadius = 1f;

        [SerializeField] private List<string> tagsToLockOn;
        [SerializeField] private float distanceToDetect = 10f;
        [SerializeField] private float maxAngle = 90f;

        private Transform currentTarget = null;
        private bool isLockedOn = false;
        private PlayerMover mover;
        private StateSwitch stateSwitch;

        private void Start()
        {
            mover = GetComponent<PlayerMover>();
            stateSwitch = GetComponent<StateSwitch>();
        }

        private void SetTarget(Transform transform)
        {
            if (currentTarget) targetGroup.RemoveMember(currentTarget);
            if (transform)
            {
                targetGroup.AddMember(transform, targetWeight, targetRadius);
                cameraStateController.LockOnTarget();
                isLockedOn = true;
            }
            else
            {
                cameraStateController.FreeCamera();
                isLockedOn = false;
            }
            currentTarget = transform;
            mover.IsStrafing = isLockedOn;
            mover.LookAt = transform;
        }

        public void LockOn()
        {
            if (stateSwitch.CurrentState != State.COMBAT)
            {
                ResetTarget();
                return;
            }
            if (isLockedOn) ResetTarget();
            else SetTarget(FindBestTarget());
        }

        public void ResetTarget()
        {
            SetTarget(null);
        }

        public void OnChangeState(State state)
        {
            if (stateSwitch.CurrentState != State.COMBAT)
            {
                ResetTarget();
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
                    Debug.Log($"Angle = {angle}");
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
