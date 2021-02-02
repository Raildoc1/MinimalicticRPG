using KG.Core;
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
        private float waypointDistance => currentScheduleEntity.point ? Vector3.Distance(currentScheduleEntity.point.transform.position, transform.position) : 0f;

        [SerializeField] private Schedule schedule;
        private int waypathIndex;
        private Schedule.Entity currentScheduleEntity;

        private bool inAction = false;

        private void Start()
        {
            stateSwitch.onAddCompanion.AddListener(SetCompanion);
        }

        private void OnDisable()
        {
            stateSwitch.onAddCompanion.RemoveListener(SetCompanion);
        }

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
            else if (Schedule.IsValid(schedule))
            {

                if (currentScheduleEntity != null && Schedule.InTimeInterval(GameTime.instance.minutes % (24 * 60), currentScheduleEntity.startTime, currentScheduleEntity.endTime))
                {

                    if (waypointDistance < waypointStopDistance)
                    {

                        if (currentScheduleEntity.actionHolder)
                        {
                            transform.position = currentScheduleEntity.actionHolder.interactionPosition.position;
                            transform.forward = currentScheduleEntity.actionHolder.interactionPosition.forward;
                            animatorProxy.GotoState(currentScheduleEntity.actionHolder.animatorStateName);
                        }

                        agent.isStopped = true;
                        animatorProxy.ResetInput();
                    }
                    else
                    {
                        agent.SetDestination(currentScheduleEntity.point.transform.position);
                        agent.isStopped = false;
                        animatorProxy.inputMagnitude = .5f;
                        mover.RotateToDirection(agent.desiredVelocity);
                    }
                }
                else
                {
                    animatorProxy.StopAction();
                    currentScheduleEntity = schedule.GetEntityByTime(GameTime.instance.minutes);
                }


            }

        }
    }
}

