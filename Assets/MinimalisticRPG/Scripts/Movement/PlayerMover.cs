using UnityEngine;
using KG.Core;

namespace KG.Movement {
    [RequireComponent(typeof(InputHandler))]
    public class PlayerMover : Mover {
        private InputHandler input;

        public Transform LookAt { set; private get; }

        protected override void Start() {
            base.Start();
            input = GetComponent<InputHandler>();
        }

        protected override void Update() {
            base.Update();

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