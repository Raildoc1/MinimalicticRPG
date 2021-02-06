using KG.Interact;
using System.Collections;
using UnityEngine;

namespace KG.AI
{
    public class CustomScheduleAction : ScheduleAction
    {

        private AIBase aiBase;
        private ActionHolder actionHolder;

        public CustomScheduleAction(ActionHolder actionHolder)
        {
            this.actionHolder = actionHolder;
        }

        public override void Exit()
        {
            aiBase.StopAction();
            aiBase.EnableNavMeshAgent(true);
        }

        public override void Init(AIBase aiBase)
        {
            this.aiBase = aiBase;
            aiBase.EnableNavMeshAgent(false);
            aiBase.MoveTo(actionHolder.transform.position);
            aiBase.transform.forward = actionHolder.transform.forward;
            aiBase.StartAction(actionHolder);
        }

        public override void Update()
        {
            aiBase.MoveTo(actionHolder.transform.position);
            aiBase.transform.forward = actionHolder.transform.forward;
        }
    }

}