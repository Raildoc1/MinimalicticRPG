using KG.Core;
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
            controller.enabled = true;
        }

        public virtual void StartAction(string actionName)
        {
            controller.enabled = false;
            animatorProxy.GotoState(actionName);
        }

    }
}

