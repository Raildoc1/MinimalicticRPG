using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
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

        public UnityEngine.Events.UnityEvent<State, State> onStateChange = new UnityEngine.Events.UnityEvent<State, State>();
        public UnityEngine.Events.UnityEvent<Transform> onAddCompanion = new UnityEngine.Events.UnityEvent<Transform>();

        private AnimatorProxy animatorProxy;

        private State _currentState = State.PEACE;

        private StateSwitch currentInterlocutor = null;

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
                    onStateChange.Invoke(_currentState, value);
                    if (!animatorProxy)
                    {
                        animatorProxy = GetComponent<AnimatorProxy>();
                    }
                    _currentState = value;
                    animatorProxy.currentState = (int)value;
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
            else
            {
                Debug.Log($"Lua.RegisterFunction");
                Lua.RegisterFunction("AddCompanion", this, SymbolExtensions.GetMethodInfo(() => AddCompanion()));
            }

            CurrentState = State.DIALOG;
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
            else
            {
                Debug.Log($"Lua.UnregisterFunction");
                Lua.UnregisterFunction("AddCompanion");
            }

            CurrentState = State.PEACE;
        }

        public void OnConversationEnd(Transform transform)
        {
            FinishDialog(true);
        }

        private void AddCompanion()
        {
            onAddCompanion?.Invoke(currentInterlocutor.transform);
        }

    }
}


