using KG.CombatCore;
using KG.Core;
using KG.Inventory;
using KG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace KG.AI
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(StateSwitch))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Equipment))]
    [RequireComponent(typeof(Combat))]
    [RequireComponent(typeof(AnimatorProxy))]
    public abstract class CombatAI : MonoBehaviour
    {

        public Transform headTransform;
        public float detectMinRadius = 10f;
        public float strafeDistance = 5f;
        public float stopDistance = 2f;
        public LayerMask tagsToDetect;

        protected NavMeshAgent agent;
        protected Transform currentTarget = null;
        protected AnimatorProxy animatorProxy;
        protected Mover mover;
        protected StateSwitch stateSwitch;
        protected Equipment equipment;
        protected Combat combat;

        protected float targetDistance => currentTarget ? Vector3.Distance(transform.position, currentTarget.position) : 0f;

        protected abstract void AttackTarget();

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animatorProxy = GetComponent<AnimatorProxy>();
            stateSwitch = GetComponent<StateSwitch>();
            agent = GetComponent<NavMeshAgent>();
            equipment = GetComponent<Equipment>();
            combat = GetComponent<Combat>();
        }

        void Update()
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

        protected abstract void OnDontHaveTarget();

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

            if (stateSwitch.CurrentState == State.PEACE)
            {
                equipment.DrawWeapon();
            }

            currentTarget = newTarget;
        }

        public virtual void RemoveTarget()
        {

            if (currentTarget == null)
            {
                return;
            }

            if (stateSwitch.CurrentState == State.COMBAT)
            {
                equipment.HideWeapon();
            }

            currentTarget = null;
        }

    }
}
