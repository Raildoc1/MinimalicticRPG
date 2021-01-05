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
        public LayerMask tagsToDetect;

        protected Transform currentTarget = null;

        protected Combat combat;

        protected float targetDistance => currentTarget ? Vector3.Distance(transform.position, currentTarget.position) : 0f;

        protected abstract void OnDontHaveTarget();

        protected virtual void AttackTarget()
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

    }
}
