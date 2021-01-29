using UnityEngine;
using KG.Core;
using System;

namespace KG.Movement
{
    [RequireComponent(typeof(AnimatorProxy))]
    [RequireComponent(typeof(StateSwitch))]
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {

        [Header("Settings")]

        [SerializeField] protected float angleError = .2f;
        [SerializeField] protected float rotationSpeed = 720f;

        [Header("Ground Snap")]

        [SerializeField] protected float snapDistace = 0.5f;
        [SerializeField] protected float pullDownMagnitude = 50f;
        [SerializeField] protected LayerMask groundLayers;

        [Header("Jump")]
        [SerializeField] protected float jumpSpeed = 2f;
        [SerializeField] protected float jumpHeight = 2f;

        protected Vector3 targetDirection;
        protected AnimatorProxy animator;
        protected StateSwitch stateSwitch;
        protected CharacterController controller;

        protected Vector3 beforeJumpVelocity = Vector3.zero;

        protected bool _isStrafing;
        protected bool _isJumping;

        //protected readonly float defaultGravity = Physics.gravity.y;

        public bool IsStrafing
        {
            get
            {
                return _isStrafing;
            }
            set
            {
                animator.isStrafing = value;
                _isStrafing = value;
            }
        }

        public bool IsJumping
        {
            get
            {
                return _isJumping;
            }

            private set
            {
                _isJumping = value;
            }

        }

        protected virtual void Awake()
        {
            animator = GetComponent<AnimatorProxy>();
            stateSwitch = GetComponent<StateSwitch>();
            controller = GetComponent<CharacterController>();
        }

        protected void OnDisable()
        {
            stateSwitch.onDialogStart.RemoveListener(LookAtTransform);
        }

        protected virtual void Start()
        {
            stateSwitch.onDialogStart.AddListener(LookAtTransform);
            targetDirection = transform.forward;
        }
        protected virtual void Update()
        {
            UpdatePosition();
            UpdateRotation();
            ProccesGravity();
        }

        private void UpdatePosition()
        {
            if (IsJumping)
            {

                if (!animator.startingJump)
                {
                    beforeJumpVelocity.y = 0f;
                }

                controller.Move(beforeJumpVelocity * Time.deltaTime);
            }
        }

        private void ProccesGravity()
        {

            var wasGrounded = animator.isGrounded;

            if (!wasGrounded)
            {
                animator.inAirTimer += Time.deltaTime;
            }

            var grounded = controller.isGrounded;

            if (animator.startingJump)
            {
                animator.isGrounded = false;
                return;
            }

            RaycastHit hit;

            Debug.DrawRay(transform.position, Vector3.down);

            if (Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit, snapDistace + 1))
            {
                controller.Move(new Vector3(0f, -pullDownMagnitude, 0f));
                IsJumping = false;

                animator.isGrounded = true;
            }
            else
            {

                if (wasGrounded)
                {
                    animator.inAirTimer = 0f;
                }

                animator.isGrounded = false;
            }

        }

        private void UpdateRotation()
        {

            if (animator.lockRotation)
            {
                return;
            }

            if (Equals(targetDirection, Vector3.zero))
            {
                targetDirection = transform.forward;
                return;
            }

            if (Vector3.Angle(transform.forward, targetDirection) > angleError)
            {
                var qLookRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qLookRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.forward = targetDirection;
            }
        }
        public void RotateToDirection(Vector3 direction, bool immediate = false)
        {
            if (immediate)
            {
                transform.forward = direction;
            }
            targetDirection = new Vector3(direction.x, 0, direction.z);
        }
        public void RotateToDirection(Vector2 direction, bool immediate = false)
        {
            RotateToDirection(new Vector3(direction.x, 0f, direction.y), immediate);
        }
        public void LookAtTransform(Transform target)
        {
            Debug.Log($"{name} looks at {target.name}");
            var direction = (target.position - transform.position).normalized;
            targetDirection = (new Vector3(direction.x, 0f, direction.z)).normalized;
        }
        public void Jump()
        {

            if (!animator.isGrounded)
            {
                return;
            }

            animator.Jump();
            IsJumping = true;
            animator.startingJump = true;
            beforeJumpVelocity = controller.velocity * jumpSpeed + Vector3.up * jumpHeight;
        }
    }
}
