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

