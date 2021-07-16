using UnityEngine;
using KG.Core;

namespace KG.Movement {
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(StateSwitch))]
    public class PlayerMover : Mover {

        private InputHandler _input;
        private Vector3 _direction = Vector3.zero;

        public Transform LookAt { set; private get; }

        protected override void OnEnable()
        {
            base.OnEnable();
            _input = GetComponent<InputHandler>();
            _input.OnJumpKeyInput += Jump;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _input.OnJumpKeyInput -= Jump;
        }

        protected override void Update() {
            base.Update();

            var state = StateSwitch.CurrentState;

            if (state != State.PEACE && state != State.COMBAT)
            {
                return;
            }

            if (IsStrafing && LookAt) {
                _direction = (LookAt.position - transform.position).normalized;
            } else if (_input.Magnitude > 0.1f){
                _direction = _input.InputDirection.normalized;
            }

            RotateToDirection(_direction, IsStrafing);
            Move(_direction, _input.Magnitude);
        }
    }
}