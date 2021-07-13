using KG.Core;
using UnityEngine;

namespace KG.Stats
{
    [RequireComponent(typeof(StateSwitch))]
    public class AddExperienceOnDestroy : MonoBehaviour
    {
        private StateSwitch _stateSwitch;

        [SerializeField] private int _experience = 0;

        private void OnEnable()
        {
            _stateSwitch = GetComponent<StateSwitch>();
            _stateSwitch.OnStateChange += OnStateChange;
        }

        private void OnDisable()
        {
            _stateSwitch.OnStateChange -= OnStateChange;
        }

        private void OnStateChange(State prev, State cur)
        {
            if (cur == State.DEAD && prev != State.DEAD)
            {
                PlayerStatsHolder.instance.AddExperience(_experience);
            }
        }
    }
}

