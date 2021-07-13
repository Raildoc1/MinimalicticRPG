using UnityEngine;

namespace KG.Core
{
    public enum State
    {
        PEACE = 0,
        COMBAT = 1,
        ACTION = 2,
        DIALOG = 3,
        KNOCKOUT = 4,
        DEAD = 5,
        INVENTORY = 6
    }

    [RequireComponent(typeof(AnimatorProxy))]
    public class StateSwitch : MonoBehaviour
    {
        private AnimatorProxy _animatorProxy;
        private State _currentState = State.PEACE;

        public StateSwitch currentInterlocutor { get; private set; } = null;

        public delegate void OnStateChangeEvent(State previous, State current);
        public event OnStateChangeEvent OnStateChange;

        public delegate void OnDialogStartEvent(Transform target);
        public event OnDialogStartEvent OnDialogStart;

        public State CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (_currentState != value)
                {
                    OnStateChange?.Invoke(_currentState, value);
                    if (!_animatorProxy)
                    {
                        _animatorProxy = GetComponent<AnimatorProxy>();
                    }
                    _currentState = value;
                    _animatorProxy.currentState = (int)value;

                }
            }
        }

        public void DrawHideWeapon()
        {
            if (CurrentState == State.COMBAT)
            {
                CurrentState = State.PEACE;
            }
            else if (CurrentState == State.PEACE)
            {
                CurrentState = State.COMBAT;
            }
        }

        public void SetCurrentState(State state)
        {
            CurrentState = state;
        }

        public void StartDialog(StateSwitch interlocutor, bool initiator)
        {
            if (currentInterlocutor)
            {
                currentInterlocutor.FinishDialog(false);
            }

            currentInterlocutor = interlocutor;

            if (initiator)
            {
                currentInterlocutor.StartDialog(this, false);
            }

            CurrentState = State.DIALOG;

            OnDialogStart?.Invoke(interlocutor.transform);
        }

        public void FinishDialog(bool initiator)
        {

            if (currentInterlocutor)
            {

                if (initiator)
                {
                    currentInterlocutor.FinishDialog(false);
                }

                currentInterlocutor = null;
            }

            if (CurrentState != State.DIALOG)
            {
                return;
            }

            CurrentState = State.PEACE;
        }

        public void OnConversationEnd(Transform transform)
        {
            FinishDialog(true);
        }

    }
}


