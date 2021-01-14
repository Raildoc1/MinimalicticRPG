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

        protected Vector3 targetDirection;
        protected AnimatorProxy animator;
        protected StateSwitch stateSwitch;
        protected CharacterController controller;

        protected bool _isStrafing;


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
            UpdateRotation();
            ProccesGravity();
        }

        private void ProccesGravity()
        {

            var grounded = controller.isGrounded;

            RaycastHit hit;

            if (Physics.Raycast(new Ray(transform.position, Vector3.down), out hit, snapDistace))
            {
                controller.Move(new Vector3(0f, -pullDownMagnitude, 0f));
            }
            else
            {
                Debug.Log("TOO FAR FROM GROUND");
            }


            Debug.Log(grounded);

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
    }
}
