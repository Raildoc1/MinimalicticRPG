using UnityEngine;
using UnityEngine.Events;

namespace KG.Core
{
    [RequireComponent(typeof(AnimatorProxy))]
    public class InputHandler : MonoBehaviour
    {

        #region PublicOrSerializableFields

        [SerializeField] private float inputSensitivity = 4f;
        [SerializeField] private float stopTime = 0.2f;
        [SerializeField] private UnityEvent OnDrawWeaponInput;
        [SerializeField] private UnityEvent OnMainKeyInput;
        [SerializeField] private UnityEvent OnLockOnKeyInput;

        public KeyCode DrawWeaponKey = KeyCode.Mouse2;
        public KeyCode MainKey = KeyCode.Mouse0;
        public KeyCode LockOn = KeyCode.Mouse1;

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

        public float RawInputMagnitude => Mathf.Max(Mathf.Abs(Input.GetAxisRaw("Vertical")), Mathf.Abs(Input.GetAxisRaw("Horizontal")));


        public Vector3 InputDirection
        {
            get
            {
                return inputDirection;
            }
        }
        #endregion

        #region PrivateOrProtectedFields
        private Vector3 inputDirection;
        private AnimatorProxy animatorProxy;
        private float _horizontalRaw = 0f;
        private float _verticalRaw = 0f;
        private float _vertical = 0f;
        private float _horizontal = 0f;
        #endregion

        #region UnityFunctions
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

        public void Update()
        {
            UpdateInputAxises();
            UpdateInputDirection();
            UpdateAnimator();
            CheckKeyInput();
        }
        #endregion

        private void UpdateAnimator()
        {
            animatorProxy.inputMagnitude = RawInputMagnitude;
        }

        private void UpdateInputAxises()
        {
            _horizontalRaw = Input.GetAxisRaw("Horizontal");
            _verticalRaw = Input.GetAxisRaw("Vertical");
            Vertical = Mathf.MoveTowards(Vertical, _verticalRaw, inputSensitivity * Time.deltaTime);
            Horizontal = Mathf.MoveTowards(Horizontal, _horizontalRaw, inputSensitivity * Time.deltaTime);
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
                OnDrawWeaponInput.Invoke();
            }
            if (Input.GetKeyDown(MainKey))
            {
                OnMainKeyInput.Invoke();
            }
            if (Input.GetKeyDown(LockOn))
            {
                OnLockOnKeyInput.Invoke();
            }
        }
    }
}