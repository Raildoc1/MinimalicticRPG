using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KG.Core
{
    [RequireComponent(typeof(AnimatorProxy))]
    [RequireComponent(typeof(StateSwitch))]
    public class InputHandler : MonoBehaviour
    {
        private Vector3 inputDirection;
        private AnimatorProxy animatorProxy;
        private StateSwitch stateSwitch;
        private float _horizontalRaw = 0f;
        private float _verticalRaw = 0f;
        private float _vertical = 0f;
        private float _horizontal = 0f;
        private bool axisInputLocked = false;

        [SerializeField] private float inputSensitivity = 4f;
        [SerializeField] private float stopTime = 0.2f;
        [SerializeField] private UnityEvent OnJumpKeyInput;

        public KeyCode DrawWeaponKey = KeyCode.Mouse2;
        public KeyCode MainKey = KeyCode.Mouse0;
        public KeyCode LockOn = KeyCode.Q;
        public KeyCode Inventory = KeyCode.Tab;
        public KeyCode Dodge = KeyCode.Mouse1;
        public KeyCode Jump = KeyCode.Space;

        public delegate void OnKeyInputEvent();
        public event OnKeyInputEvent OnDrawWeaponInput;
        public event OnKeyInputEvent OnInventoryInput;
        public event OnKeyInputEvent OnMainKeyInput;
        public event OnKeyInputEvent DodgeOnKeyInput;
        public event OnKeyInputEvent OnLockOnKeyInput;

        public float Vertical
        {
            private set
            {
                if (!animatorProxy) GetAnimator();
                animatorProxy.inputVertical = value;
                _vertical = value;
            }
            get
            {
                return _vertical;
            }
        }

        public float Horizontal
        {
            private set
            {
                if (!animatorProxy) GetAnimator();
                animatorProxy.inputHorizontal = value;
                _horizontal = value;
            }
            get
            {
                return _horizontal;
            }
        }

        public float Magnitude
        {
            get
            {
                return Mathf.Clamp01((new Vector2(Vertical, Horizontal)).magnitude);
            }
        }

        public float RawInputMagnitude => axisInputLocked ? 0f : Mathf.Max(Mathf.Abs(Input.GetAxisRaw("Vertical")), Mathf.Abs(Input.GetAxisRaw("Horizontal")));


        public Vector3 InputDirection
        {
            get
            {
                return inputDirection;
            }
        }

        private void Awake()
        {
            stateSwitch = GetComponent<StateSwitch>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GetAnimator();
        }

        private void GetAnimator()
        {
            animatorProxy = GetComponent<AnimatorProxy>();
        }

        private void Update()
        {
            UpdateInputAxises();
            UpdateInputDirection();
            UpdateAnimator();
            CheckKeyInput();
        }

        public void LockInputAxisInput(float time)
        {
            axisInputLocked = true;
            StartCoroutine(LockInputRoutine(time));
        }

        public IEnumerator LockInputRoutine(float time)
        {
            yield return new WaitForSeconds(time);
            axisInputLocked = false;
        }

        private void UpdateAnimator()
        {
            if (stateSwitch.CurrentState == State.COMBAT || stateSwitch.CurrentState == State.PEACE)
            {
                animatorProxy.inputMagnitude = RawInputMagnitude;
            }
            else
            {
                animatorProxy.inputMagnitude = 0f;
            }
        }

        private void UpdateInputAxises()
        {
            if (!axisInputLocked && (stateSwitch.CurrentState == State.COMBAT || stateSwitch.CurrentState == State.PEACE))
            {
                _horizontalRaw = Input.GetAxisRaw("Horizontal");
                _verticalRaw = Input.GetAxisRaw("Vertical");
                Vertical = Mathf.MoveTowards(Vertical, _verticalRaw, inputSensitivity * Time.deltaTime);
                Horizontal = Mathf.MoveTowards(Horizontal, _horizontalRaw, inputSensitivity * Time.deltaTime);
            }
            else
            {
                Vertical = Mathf.MoveTowards(Vertical, 0f, inputSensitivity * Time.deltaTime);
                Horizontal = Mathf.MoveTowards(Horizontal, 0f, inputSensitivity * Time.deltaTime);
            }

        }

        private void UpdateInputDirection()
        {
            if (!Camera.main)
            {
                Debug.LogError("Can't update direction because no main camera found!");
                return;
            }
            Transform cam = Camera.main.transform;
            inputDirection = cam.forward * Vertical + cam.right * Horizontal;
            inputDirection.y = 0;
            inputDirection = Vector3.Normalize(inputDirection);
        }

        private void CheckKeyInput()
        {
            if (Input.GetKeyDown(DrawWeaponKey))
            {
                OnDrawWeaponInput?.Invoke();
            }
            if (Input.GetKeyDown(MainKey))
            {
                OnMainKeyInput?.Invoke();
            }
            if (Input.GetKeyDown(LockOn))
            {
                OnLockOnKeyInput?.Invoke();
            }
            if (Input.GetKeyDown(Inventory))
            {
                OnInventoryInput?.Invoke();
            }
            if (Input.GetKeyDown(Dodge))
            {
                DodgeOnKeyInput?.Invoke();
            }
            if (Input.GetKeyDown(Jump))
            {
                OnJumpKeyInput?.Invoke();
            }
        }
    }
}