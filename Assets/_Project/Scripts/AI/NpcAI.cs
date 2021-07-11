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
        private ScheduleAction action;

        private bool inAction = false;

        public void SetInterlocutorAsCompanion()
        {
            companion = stateSwitch.currentInterlocutor.transform;
        }

        public void SetCompanion(Transform companion)
        {
            this.companion = companion;
        }

        public void ResetCompanion()
        {
            SetCompanion(null);
        }

        protected override void AttackTarget()
        {
            base.AttackTarget();

            if (action != null)
            {
                StopScheduleAction();
            }
        }

        protected override void OnDontHaveTarget()
        {

            var state = stateSwitch.CurrentState;

            if (state != Core.State.PEACE)
            {
                StopScheduleAction();

                agent.isStopped = true;
                animatorProxy.ResetInput();
            }
            else if (companion)
            {
                StopScheduleAction();

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

                    if (action != null)
                    {
                        agent.isStopped = true;
                        animatorProxy.ResetInput();
                        action.Update();
                    }
                    else if (waypointDistance < waypointStopDistance)
                    {

                        if (currentScheduleEntity.actionHolder)
                        {
                            var newAction = new CustomScheduleAction(currentScheduleEntity.actionHolder);
                            newAction.Init(this);
                            action = newAction;
                        }
                        else
                        {
                            var newAction = new IdleScheduleAction(currentScheduleEntity.point.transform);
                            newAction.Init(this);
                            action = newAction;
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
                    StopScheduleAction();
                    currentScheduleEntity = schedule.GetEntityByTime(GameTime.instance.minutes);
                    Debug.Log($"new currentScheduleEntity = {currentScheduleEntity}");
                }


            }

        }

        private void StopScheduleAction()
        {
            action?.Exit();
            action = null;
        }
    }
}

