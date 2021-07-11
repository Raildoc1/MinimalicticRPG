using UnityEngine;

namespace KG.AI
{
    public class IdleScheduleAction : ScheduleAction
    {

        private AIBase aiBase;
        private Transform targetTransform;

        public IdleScheduleAction(Transform targetTransform)
        {

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
        }

        public override void Update()
        {
            aiBase.MoveTo(targetTransform.position);
            aiBase.RotateToDirection(targetTransform.forward);
        }
    }

}