using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.AI
{
    public class NpcAI : CombatAI
    {
        public float companionStopDistance = 2f;
        public float waypointStopDistance = 0.2f;

        private Transform companion;
        private float companionDistance => companion ? Vector3.Distance(companion.position, transform.position) : 0f;
        private float waypointDistance => currentWaypoint ? Vector3.Distance(currentWaypoint.transform.position, transform.position) : 0f;

        [SerializeField] private Waypath waypath;
        private int waypathIndex;
        private Waypoint currentWaypoint;

        public void SetCompanion(Transform companion)
        {
            this.companion = companion;
        }

        protected override void OnDontHaveTarget()
        {

            var state = stateSwitch.CurrentState;
            
            if (state != Core.State.PEACE)
            {
                agent.isStopped = true;
                animatorProxy.ResetInput();
            }
            else if (companion)
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
            else if (waypath)
            {
                if (!currentWaypoint)
                {
                    currentWaypoint = waypath.GetNextPoint();
                }

                if (waypointDistance < waypointStopDistance)
                {
                    currentWaypoint = waypath.GetNextPoint();
                }

                agent.SetDestination(currentWaypoint.transform.position);
                agent.isStopped = false;
                animatorProxy.inputMagnitude = 1f;
                mover.RotateToDirection(agent.desiredVelocity);

            }

        }
    }
}

