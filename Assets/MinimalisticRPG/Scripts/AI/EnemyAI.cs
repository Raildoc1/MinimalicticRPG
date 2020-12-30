using KG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.AI
{
    [RequireComponent(typeof(Mover))]
    public class EnemyAI : MonoBehaviour
    {

        public Transform headTransform;
        public float checkForTargetsInterval = 0.25f;
        public float detectMinRadius = 5f;
        public LayerMask tagsToDetect;

        private Transform currentTarget = null;
        private Mover mover;
        private Animator animator;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine(CheckForTargetsRoutine());
        }

        void Update()
        {
            if (!currentTarget)
            {
                animator.SetFloat("InputMagnitude", 0f, mover.stopTime, Time.deltaTime);
                return;
            }

            animator.SetFloat("InputMagnitude", 1f, mover.stopTime, Time.deltaTime);
            mover.RotateToDirection((currentTarget.position - transform.position).normalized);
        }

        private void SetTarget(Transform newTarget)
        {
            currentTarget = newTarget;
        }

        private void RemoveTarget()
        {
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
