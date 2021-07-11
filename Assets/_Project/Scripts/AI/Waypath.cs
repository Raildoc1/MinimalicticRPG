using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.AI
{
    public class Waypath : MonoBehaviour
    {
        public List<Waypoint> waypoints;
        private int index = 0;

        private void OnDrawGizmos()
        {
            if (waypoints.Count < 1)
            {
                return;
            }

            for (int i = 1; i < waypoints.Count; i++)
            {
                Gizmos.DrawLine(waypoints[i-1].transform.position, waypoints[i].transform.position);
            }
        }

        public Waypoint GetNextPoint()
        {
            index = (index + 1) % waypoints.Count;

            return waypoints[index];
        }
    }
}
