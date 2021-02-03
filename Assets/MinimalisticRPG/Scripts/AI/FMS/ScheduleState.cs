namespace KG.AI.FSM
{
    public abstract class ScheduleState
    {
        public abstract void Init(AIBase aiBase);
        public abstract void Update();
        public abstract void Exit();
    }
}
