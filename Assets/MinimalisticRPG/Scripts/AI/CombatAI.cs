using KG.CombatCore;
using KG.Core;
using UnityEngine;

namespace KG.AI
{

    [RequireComponent(typeof(Combat))]
    public abstract class CombatAI : AIBase
    {
        [Header("References")]
        public Transform headTransform;

        [Header("Detection")]
        public LayerMask tagsToDetect;
        public float detectMinRadius = 10f;
        public float strafeDistance = 5f;
        public float stopDistance = 2f;
        public float tooCloseDistance = 1f;

        [Header("Outflank")]
        public float outflankMinTime = 1f;
        public float outflankMaxTime = 2f;

        private float outflankTime = -1f;
        private bool outflankDirection = true;

        [Header("Attack")]
        public float attackMinTime = 1f;
        public float attackMaxTime = 2f;

        private float attackTime = -1f;

        [Header("Probabilities")]
        [Range(0f, 1f)]
        public float outFlankProbability = 0.25f;
        [Range(0f, 1f)]
        public float dodgeProbability = 0.25f;
        [Range(0f, 1f)]
        public float dodgeWhileGettingDamgeProbability = 0.5f;
        [Range(0f, 1f)]
        public float dodgeWhileTargetAttacksProbability = 0.5f;


        protected Transform currentTarget = null;
        protected AnimatorProxy targetAnimator = null;

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


            if (animatorProxy.gettingDamage)
            {
                attackTime = -1f;
                outflankTime = -1f;

                if (!animatorProxy.cannotBlock)
                {
                    if (CheckProbability(dodgeWhileGettingDamgeProbability))
                    {
                        DodgeBackwards();
                    }
                }

            } 
            else if (targetAnimator.enteringAttack && CheckProbability(dodgeWhileTargetAttacksProbability))
            {
                DodgeBackwards();
            }
            else if (attackTime > 0f)
            {
                attackTime -= Time.deltaTime;
                combat.Attack();

                mover.RotateToDirection((currentTarget.position - transform.position).normalized);
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
                        attackTime = Random.Range(attackMinTime, attackMaxTime);
                        break;
                    case CombatAction.OUTFLANK:
                        outflankTime = Random.Range(outflankTime, outflankMaxTime);
                        outflankDirection = CheckProbability(0.5f);
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

        private void DodgeBackwards()
        {
            animatorProxy.inputVertical = -1f;
            animatorProxy.inputHorizontal = 0f;
            combat.Dodge();
        }

        private bool CheckProbability(float chance)
        {
            return Random.Range(0f, 1f) < chance;
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
            targetAnimator = currentTarget.GetComponent<AnimatorProxy>();
        }

        public virtual void RemoveTarget()
        {

            if (currentTarget == null)
            {
                return;
            }

            stateSwitch.CurrentState = State.PEACE;

            currentTarget = null;
            targetAnimator = null;
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
