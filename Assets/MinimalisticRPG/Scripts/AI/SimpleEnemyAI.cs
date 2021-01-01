using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KG.AI
{

    public class SimpleEnemyAI : CombatAI
    {

        public float checkForTargetsInterval = 0.25f;
        private Coroutine checkForTargetsRoutine;

        private void Start()
        {
            checkForTargetsRoutine = StartCoroutine(CheckForTargetsRoutine());
        }

        private void OnDisable()
        {
            if (checkForTargetsRoutine != null)
            {
                StopCoroutine(checkForTargetsRoutine);
            }
        }

        protected override void AttackTarget()
        {
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
        }


        protected override void OnDontHaveTarget()
        {
            animatorProxy.ResetInput();
            animatorProxy.isStrafing = false;
            agent.SetDestination(transform.position);
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

