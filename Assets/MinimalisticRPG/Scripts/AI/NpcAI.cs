using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.AI
{
    public class NpcAI : CombatAI
    {
        public Transform companion;
        public float companionStopDistance = 2f;

        private float companionDistance => companion ? Vector3.Distance(companion.position, transform.position) : 0f;

        protected override void OnDontHaveTarget()
        {
            if (companion)
            {
                agent.SetDestination(companion.position);

                if (companionDistance < companionStopDistance)
                {
                    agent.isStopped = true;
                    animatorProxy.ResetInput();
                    mover.RotateToDirection((companion.position - transform.position).normalized);

                }
                else
                {
                    agent.isStopped = false;
                    animatorProxy.inputMagnitude = 1f;
                    mover.RotateToDirection(agent.desiredVelocity);
                }

            }

        }
    }
}

