using KG.CombatCore;
using KG.Core;
using KG.Inventory;
using KG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KG.AI
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(StateSwitch))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(Combat))]
    [RequireComponent(typeof(AnimatorProxy))]
    public class EnemyAI : MonoBehaviour
    {

        public Transform headTransform;
        public float checkForTargetsInterval = 0.25f;
        public float detectMinRadius = 10f;
        public float strafeDistance = 5f;
        public float stopDistance = 2f;
        public LayerMask tagsToDetect;

        private Transform currentTarget = null;
        private Mover mover;
        private AnimatorProxy animatorProxy;
        private StateSwitch stateSwitch;
        private NavMeshAgent agent;
        private Equipment equipment;
        private Combat combat;

        private float targetDistance => currentTarget ? Vector3.Distance(transform.position, currentTarget.position) : 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animatorProxy = GetComponent<AnimatorProxy>();
            stateSwitch = GetComponent<StateSwitch>();
            agent = GetComponent<NavMeshAgent>();
            equipment = GetComponent<Equipment>();
            combat = GetComponent<Combat>();
        }

        private void Start()
        {
            StartCoroutine(CheckForTargetsRoutine());
        }

        void Update()
        {
            if (!currentTarget)
            {
                animatorProxy.ResetInput();
                animatorProxy.isStrafing = false;
                agent.SetDestination(transform.position);
                return;
            }

            agent.SetDestination(currentTarget.position);

            if (targetDistance < stopDistance)
            {
                agent.isStopped = true;
                animatorProxy.ResetInput();
                animatorProxy.isStrafing = true;
                combat.Attack();
                mover.RotateToDirection((currentTarget.position - transform.position).normalized);

            }
            else if (targetDistance < strafeDistance)
            {
                agent.isStopped = false;
                animatorProxy.isStrafing = true;

                animatorProxy.SetAxisInput(new Vector2(0f, 1f));

                mover.RotateToDirection(agent.desiredVelocity);
            }
            else if (targetDistance < detectMinRadius)
            {
                agent.isStopped = false;
                animatorProxy.isStrafing = false;
                animatorProxy.inputMagnitude = 1f;
                mover.RotateToDirection(agent.desiredVelocity);
            }
            else
            {
                agent.isStopped = true;
                animatorProxy.isStrafing = false;
                animatorProxy.ResetInput();
                mover.RotateToDirection((currentTarget.position - transform.position).normalized);
            }

            //mover.RotateToDirection((currentTarget.position - transform.position).normalized);
        }

        private void SetTarget(Transform newTarget)
        {
            if (!newTarget)
            {
                RemoveTarget();
                return;
            }
            else if (newTarget.Equals(currentTarget))
            {
                return;
            }

            if (stateSwitch.CurrentState == State.PEACE)
            {
                equipment.DrawWeapon();
            }

            currentTarget = newTarget;
        }

        private void RemoveTarget()
        {

            if (currentTarget == null)
            {
                return;
            }

            if (stateSwitch.CurrentState == State.COMBAT)
            {
                equipment.HideWeapon();
            }

            currentTarget = null;
        }

        private Transform FindClosestTarget(Collider[] colliders)
        {
            if (colliders.Length == 0)
            {
                return null;
            }

            var closest = colliders[0].transform;
            var minDistance = Vector3.Distance(transform.position, closest.position);

            for (int i = 1; i < colliders.Length; i++)
            {
                var distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = colliders[i].transform;
                }
            }

            return closest;
        }

        private IEnumerator CheckForTargetsRoutine()
        {
            while (true)
            {

                yield return new WaitForSeconds(checkForTargetsInterval);

                var targets = Physics.OverlapSphere(headTransform.position, detectMinRadius, tagsToDetect);

                if (targets.Length == 0)
                {
                    RemoveTarget();
                }
                else
                {
                    SetTarget(FindClosestTarget(targets));
                }
            }

        }
    }
}
