using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.AI
{
    public class Waypoint : MonoBehaviour
    {

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
        }

    }
}
