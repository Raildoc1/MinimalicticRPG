using KG.Interact;
using System.Collections.Generic;

namespace KG.AI
{
    [System.Serializable]
    public class Schedule
    {

        public List<Entity> list;

        public Entity GetEntityByTime(float time)
        {

            time %= 60 * 24;

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (time >= list[i].startTime && time < list[i + 1].startTime)
                {
                    return list[i];
                }
            }

            return list[list.Count - 1];
        }


        public static bool InTimeInterval(float time, float start, float end)
        {

            if (start <= end)
            {
                return time >= start && time <= end;
            }
            else
            {
                return time > start || time < end;
            }
        }

        public static bool IsValid(Schedule? schedule)
        {
            return schedule != null && schedule.list.Count > 0;
        }

        [System.Serializable]
        public class Entity
        {
            public Waypoint point;
            public ActionHolder actionHolder;
            public float startTime;
            public float endTime;
        }
    }
}

