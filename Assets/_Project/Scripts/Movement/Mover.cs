using UnityEngine;
using KG.Core;
using System;
using System.Collections;

namespace KG.Movement
{
    [RequireComponent(typeof(AnimatorProxy))]
    [RequireComponent(typeof(StateSwitch))]
    [RequireComponent(typeof(CharacterController))]
    public class Mover : MonoBehaviour
    {
        private Vector3 _targetDirection;
        private AnimatorProxy _animator;
        private StateSwitch _stateSwitch;
        private CharacterController _controller;
        private float _inAirTime = 0f;
        private bool _startJump = false;

        private Vector3 _beforeJumpVelocity = Vector3.zero;
        private Vector3 _playerVelocity;

        private bool _isStrafing;
        private bool _isJumping;
        private bool _isGrounded;
        private bool _falling;

        [Header("Settings")]
        [SerializeField] protected float angleError = .2f;
        [SerializeField] protected float rotationSpeed = 720f;
        [SerializeField] protected float maxSpeed = 5f;

        [Header("Ground Snap")]
        [SerializeField] protected float snapDistace = 0.5f;
        [SerializeField] protected float pullDownMagnitude = 50f;
        [SerializeField] protected LayerMask groundLayers;

        [Header("Jump")]
        [SerializeField] protected float jumpSpeed = 2f;
        [SerializeField] protected float jumpHeight = 2f;
        [SerializeField] protected float startJumpTime = .25f;

        private readonly float defaultGravity = Physics.gravity.y;

        protected StateSwitch StateSwitch => _stateSwitch;

        public bool IsStrafing
        {
            get
            {
                return _isStrafing;
            }
            set
            {
                _animator.isStrafing = value;
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

        public bool IsGrounded
        {
            get
            {
                return _isGrounded;
            }

            private set
            {
                //_animator.isGrounded = value;
                _isGrounded = value;
            }

        }

        public bool CanMove => IsGrounded && !_animator.InAttack && !_animator.BlockMovement && !_animator.Landing && !_startJump && !_animator.inDodge;

        protected virtual void Awake()
        {
            _animator = GetComponent<AnimatorProxy>();
            _controller = GetComponent<CharacterController>();
        }

        protected virtual void OnEnable()
        {
            _stateSwitch = GetComponent<StateSwitch>();
            _stateSwitch.OnDialogStart += LookAtTransform;
        }

        protected virtual void OnDisable()
        {
            _stateSwitch.OnDialogStart -= LookAtTransform;
        }

        protected virtual void Start()
        {
            _targetDirection = transform.forward;
        }

        protected virtual void Update()
        {
            UpdatePosition();
            UpdateRotation();
            ProccesGravity();

            if (!IsGrounded)
            {
                _inAirTime += Time.deltaTime;

                if (_inAirTime > 0.15f)
                {
                    _falling = true;
                }
            }
            else
            {
                _falling = false;
                _inAirTime = 0f;
                _animator.inAirTimer = 0f;
            }

            _animator.isGrounded = !_falling;
        }

        private void UpdatePosition()
        {
            if (!_controller.enabled)
            {
                return;
            }

            if ((!IsGrounded || _startJump) && !_animator.Landing)
            {
                _controller.Move(_playerVelocity * Time.deltaTime);
                _playerVelocity += Physics.gravity * Time.deltaTime;
            }
            else
            {
                _controller.Move(Physics.gravity * Time.deltaTime);
            }
        }

        public void Move(Vector3 direction, float proportion)
        {
            if (!CanMove)
            {
                return;
            }
            _playerVelocity = proportion * maxSpeed * direction;
            _controller.Move(Time.deltaTime * _playerVelocity);
        }

        public void MoveToTick(Vector3 position)
        {
            var direction = position - transform.position;

            _controller.Move(direction.normalized * Time.deltaTime);
        }

        private void ProccesGravity()
        {
            if (_falling)
            {
                _animator.inAirTimer += Time.deltaTime;
            }

            RaycastHit hit;

            IsGrounded = Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out hit, snapDistace + 1);
        }

        private void UpdateRotation()
        {

            if (_animator.lockRotation)
            {
                return;
            }

            if (Equals(_targetDirection, Vector3.zero))
            {
                _targetDirection = transform.forward;
                return;
            }

            if (Vector3.Angle(transform.forward, _targetDirection) > angleError)
            {
                var qLookRotation = Quaternion.LookRotation(_targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qLookRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.forward = _targetDirection;
            }
        }

        public void RotateToDirection(Vector3 direction, bool immediate = false)
        {
            if (immediate)
            {
                transform.forward = direction;
            }
            _targetDirection = new Vector3(direction.x, 0, direction.z);
        }

        public void RotateToDirection(Vector2 direction, bool immediate = false)
        {
            RotateToDirection(new Vector3(direction.x, 0f, direction.y), immediate);
        }

        public void LookAtTransform(Transform target)
        {
            var direction = (target.position - transform.position).normalized;
            _targetDirection = (new Vector3(direction.x, 0f, direction.z)).normalized;
        }

        public void Jump()
        {
            if (!IsGrounded || StateSwitch.CurrentState != State.PEACE)
            {
                return;
            }

            _animator.Jump();
            IsJumping = true;
            _animator.startingJump = true;

            StartCoroutine(StartJumpRoutine());

            var velocity = _controller.velocity;
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * 2f * 9.81f);
        }

        private void OnAnimatorMove()
        {
            if (_animator.InAttack || _animator.inDodge)
            {
                _animator.ApplyRootMotion();
            }
        }

        private IEnumerator StartJumpRoutine()
        {
            _startJump = true;
            yield return new WaitForSeconds(startJumpTime);
            _startJump = false;
        }
    }
}
