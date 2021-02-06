using KG.Core;
using KG.Interact;
using KG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace KG.AI
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(StateSwitch))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AnimatorProxy))]
    [RequireComponent(typeof(CharacterController))]
    public class AIBase : MonoBehaviour
    {
        protected AnimatorProxy animatorProxy;
        protected NavMeshAgent agent;
        protected StateSwitch stateSwitch;
        protected Mover mover;
        protected CharacterController controller;

        protected ActionHolder actionHolder;

        protected virtual void Awake()
        {
            mover = GetComponent<Mover>();
            animatorProxy = GetComponent<AnimatorProxy>();
            stateSwitch = GetComponent<StateSwitch>();
            agent = GetComponent<NavMeshAgent>();
            controller = GetComponent<CharacterController>();
        }

        public virtual void StopAction()
        {
            animatorProxy.StopAction();

            if (actionHolder)
            {
                actionHolder.colliderToDisable.enabled = true;
            }

            actionHolder = null;
        }

        public virtual void StartAction(ActionHolder actionHolder)
        {
            if (actionHolder)
            {
                StopAction();
            }

            this.actionHolder = actionHolder;
            actionHolder.colliderToDisable.enabled = false;
            animatorProxy.GotoState(actionHolder.animatorStateName);
        }

        public virtual void MoveTo(Vector3 position)
        {
            mover.MoveToTick(position);
        }

        public virtual void RotateToDirection(Vector3 direction)
        {
            mover.RotateToDirection(direction);
        }

        public void EnableNavMeshAgent(bool enabled = true)
        {
            agent.enabled = enabled;
        }

    }
}

