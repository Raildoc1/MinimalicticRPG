using KG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Stats
{
    [RequireComponent(typeof(StateSwitch))]
    public class AddExperienceOnDestroy : MonoBehaviour
    {

        private StateSwitch stateSwitch;

        public int experience = 0;

        private void Awake()
        {
            stateSwitch = GetComponent<StateSwitch>();
        }

        private void Start()
        {
            stateSwitch.onStateChange.AddListener(OnStateChange);
        }

        private void OnDisable()
        {
            stateSwitch.onStateChange.RemoveListener(OnStateChange);
        }

        private void OnStateChange(State prev, State cur)
        {

            if (cur == State.DEAD && prev != State.DEAD)
            {
                PlayerStatsHolder.instance.AddExperience(experience);
            }

        }

    }
}

