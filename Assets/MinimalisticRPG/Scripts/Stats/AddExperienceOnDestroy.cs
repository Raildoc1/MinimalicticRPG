using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Stats
{
    public class AddExperienceOnDestroy : MonoBehaviour
    {

        public int experience = 0;

        private void OnDestroy()
        {
            PlayerStatsHolder.instance.AddExperience(experience);
        }

    }
}

