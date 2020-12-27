using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Core {
    public enum State {
        PEACE = 0,
        COMBAT = 1,
        ACTION = 2,
        DIALOG = 3,
        KNOCKOUT = 4,
        DEAD = 5
    }

    [RequireComponent(typeof(Animator))]
    public class StateSwitch : MonoBehaviour {
        
        [System.Serializable]
        public class OnStateChangeEvent : UnityEngine.Events.UnityEvent<State> {}
        public OnStateChangeEvent onStateChange = new OnStateChangeEvent();

        private Animator animator;

        private State _currentState = State.PEACE;

        public State CurrentState {
            get{
                return _currentState;
            }
            set{
                if(_currentState != value) {
                    _currentState = value;
                    onStateChange.Invoke(_currentState);
                    if (!animator) animator = GetComponent<Animator>();
                    animator.SetInteger("CurrentState", (int)value);
                }
            }
        }

    }
}


