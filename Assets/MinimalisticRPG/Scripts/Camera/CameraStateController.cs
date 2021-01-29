using Cinemachine;
using KG.Core;
using UnityEngine;

namespace KG.CameraControl
{
    public class CameraStateController : MonoBehaviour
    {

        [SerializeField] private GameObject freeCamera;
        [SerializeField] private GameObject lockCamera;
        [SerializeField] private GameObject dialogCamera;
        [SerializeField] private GameObject inventoryCamera;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform cameraTransform;

        private CinemachineFreeLook freeLookCamera;

        private StateSwitch playerStateSwtich;

        private void Awake()
        {
            freeLookCamera = freeCamera.GetComponent<CinemachineFreeLook>();

            if (!freeLookCamera)
            {
                Debug.LogError("No CinemachineFreeLook component on freeCamera");
            }
        }

        private void Start()
        {
            playerStateSwtich = playerTransform.GetComponent<StateSwitch>();

            if (!playerStateSwtich)
            {
                Debug.LogError($"Player do not have StateSwtich component!");
            }

            playerStateSwtich.onStateChange.AddListener(OnChangeState);


            freeLookCamera.m_XAxis.Value = 0f;
            freeLookCamera.m_YAxis.Value = 0f;
        }

        private void OnDisable()
        {
            playerStateSwtich.onStateChange.RemoveListener(OnChangeState);
        }

        public void LockOnTarget()
        {
            lockCamera.SetActive(true);
            freeCamera.SetActive(false);
            dialogCamera.SetActive(false);
            inventoryCamera.SetActive(false);
        }

        public void UnlockTarget(State state = State.PEACE)
        {
            lockCamera.SetActive(false);

            switch (state)
            {
                case State.DIALOG:
                    StartDialog();
                    break;
                case State.INVENTORY:
                    OpenInventory();
                    break;
                default:
                    FreeCamera();
                    break;
            }
        }

        public void FreeCamera()
        {
            freeCamera.SetActive(true);
            lockCamera.SetActive(false);
            dialogCamera.SetActive(false);
            inventoryCamera.SetActive(false);
        }

        public void StartDialog()
        {
            dialogCamera.SetActive(true);
            freeCamera.SetActive(false);
            lockCamera.SetActive(false);
            inventoryCamera.SetActive(false);
        }

        public void OpenInventory()
        {
            inventoryCamera.SetActive(true);
            freeCamera.SetActive(false);
            lockCamera.SetActive(false);
            dialogCamera.SetActive(false);
        }

        public void OnChangeState(State _, State currentState)
        {

            Debug.Log($"OnChangeState(_, {currentState})");

            if (currentState == State.DIALOG)
            {
                StartDialog();
            }
            else if (currentState == State.PEACE)
            {
                FreeCamera();
            }
            else if (currentState == State.INVENTORY)
            {
                OpenInventory();
            }
        }
    }
}

