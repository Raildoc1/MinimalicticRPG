using UnityEngine;
using KG.Core;

namespace KG.Movement {
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(StateSwitch))]
    public class PlayerMover : Mover {
        private InputHandler input;

        public Transform LookAt { set; private get; }

        private StateSwitch stateSwitch;

        protected void Awake()
        {
            stateSwitch = GetComponent<StateSwitch>();
        }

        protected override void Start() {
            base.Start();
            input = GetComponent<InputHandler>();
        }

        protected override void Update() {
            base.Update();

            var state = stateSwitch.CurrentState;

            if (state != State.PEACE && state != State.COMBAT)
            {
                return;
            }

            Vector3 direction = transform.forward;

            if (IsStrafing && LookAt) {
                direction = (LookAt.position - transform.position).normalized;
            } else if (input.Magnitude > 0.1f){
                direction = input.InputDirection.normalized;
            }

            RotateToDirection(direction, IsStrafing);
        }
    }
}