using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.AI.FSM
{
    public class CustomActionState : ScheduleState
    {

        private readonly string actionName;
        private AIBase aiBase;

        public CustomActionState(string actionName)
        {
            this.actionName = actionName;
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
            throw new System.NotImplementedException();
        }
    }
}

