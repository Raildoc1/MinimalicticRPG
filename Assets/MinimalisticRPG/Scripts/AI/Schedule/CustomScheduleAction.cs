using UnityEngine;

namespace KG.AI
{
    public class CustomScheduleAction : ScheduleAction
    {

        private AIBase aiBase;
        private Transform targetTransform;
        private string actionName;

        public CustomScheduleAction(string actionName, Transform targetTransform)
        {

            this.actionName = actionName;
            this.targetTransform = targetTransform;

            var forward = targetTransform.forward;
            forward.y = 0;
            targetTransform.forward = forward;

        }

        public override void Exit()
        {
            aiBase.StopAction();
        }

        public override void Init(AIBase aiBase)
        {
            this.aiBase = aiBase;
            aiBase.transform.position = targetTransform.position;
            aiBase.transform.forward = targetTransform.forward;
            aiBase.StartAction(actionName);
        }

        public override void Update()
        {
            aiBase.transform.position = targetTransform.position;
            aiBase.transform.forward = targetTransform.forward;
        }
    }

}