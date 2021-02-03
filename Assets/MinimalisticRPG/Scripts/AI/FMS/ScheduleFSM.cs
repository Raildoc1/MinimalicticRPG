namespace KG.AI.FSM
{
    public class ScheduleFSM
    {
        private readonly AIBase aiBase;

        private ScheduleState currentState;

        public ScheduleFSM(AIBase aiBase)
        {
            this.aiBase = aiBase;
        }

        public void ChangeState(ScheduleState state)
        {
            currentState?.Exit();

            currentState = state;

            state.Init(aiBase);
        }

        public void UpdateState()
        {
            currentState?.Update();
        }
    }
}

