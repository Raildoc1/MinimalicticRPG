namespace KG.AI
{
    public abstract class ScheduleAction
    {
        public abstract void Init(AIBase aiBase);
        public abstract void Update();
        public abstract void Exit();
    }
}

