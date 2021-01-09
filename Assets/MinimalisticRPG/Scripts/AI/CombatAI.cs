using KG.CombatCore;
using KG.Core;
using UnityEngine;

namespace KG.AI
{

    [RequireComponent(typeof(Combat))]
    public abstract class CombatAI : AIBase
    {

        public Transform headTransform;
        public float detectMinRadius = 10f;
        public float strafeDistance = 5f;
        public float stopDistance = 2f;
        public float tooCloseDistance = 1f;
        public LayerMask tagsToDetect;
        public float outflankTime = -1f;
        public bool outflankDirection = true;

        [Range(0f, 1f)]
        public float outFlankProbability = 0.25f;
        [Range(0f, 1f)]
        public float dodgeProbability = 0.25f;

        protected Transform currentTarget = null;

        protected Combat combat;

        protected float targetDistance => currentTarget ? Vector3.Distance(transform.position, currentTarget.position) : 0f;

        protected abstract void OnDontHaveTarget();

        protected virtual void AttackTarget()
        {

            if (animatorProxy.isDead || stateSwitch.CurrentState != State.COMBAT)
            {
                return;
            }

            agent.SetDestination(currentTarget.position);

            if (animatorProxy.inDodge)
            {
                return;
            }
            else if (outflankTime > 0f)
            {
                agent.isStopped = true;
                outflankTime -= Time.deltaTime;
                animatorProxy.inputHorizontal = outflankDirection ? 1f : -1f;

                animatorProxy.inputVertical = targetDistance < tooCloseDistance ? -1f : 0f;

                mover.RotateToDirection((currentTarget.position - transform.position).normalized);
            }
            else if (targetDistance < stopDistance)
            {
                agent.isStopped = true;
                animatorProxy.ResetInput();
                animatorProxy.isStrafing = true;

                var temp = Random.Range(0f, 1f);
                CombatAction action;

                if (temp < outFlankProbability)
                {
                    action = CombatAction.OUTFLANK;
                }
                else if (temp < dodgeProbability + outFlankProbability)
                {
                    action = CombatAction.DODGE;
                }
                else
                {
                    action = CombatAction.ATTACK;
                }

                //Debug.Log($"Generated {temp}; Action {action}");

                switch (action)
                {
                    case CombatAction.NONE:
                        break;
                    case CombatAction.ATTACK:
                        combat.Attack();
                        break;
                    case CombatAction.OUTFLANK:
                        outflankTime = Random.Range(2f, 4f);
                        outflankDirection = Random.Range(0f, 1f) > 0.5f;
                        break;
                    case CombatAction.DODGE:
                        combat.Dodge();
                        break;
                    default:
                        break;
                }

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

        protected virtual void Update()
        {
            if (!currentTarget)
            {
                OnDontHaveTarget();
            }
            else
            {
                AttackTarget();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            combat = GetComponent<Combat>();
        }


        public virtual void SetTarget(Transform newTarget)
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

            stateSwitch.CurrentState = State.COMBAT;

            currentTarget = newTarget;
        }

        public virtual void RemoveTarget()
        {

            if (currentTarget == null)
            {
                return;
            }

            stateSwitch.CurrentState = State.PEACE;

            currentTarget = null;
        }


        private enum CombatAction
        {
            NONE = 0,
            ATTACK = 1,
            OUTFLANK = 2,
            DODGE = 3
        }

    }
}
