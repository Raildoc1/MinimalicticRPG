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

        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform cameraTransform;

        private CinemachineFreeLook freeLookCamera;
        private Vector3 cameraDefaultForward;
        private Vector3 cameraDefaultRight;
        private Vector3 cameraDefaultUp;

        private void Awake()
        {
            freeLookCamera = freeCamera.GetComponent<CinemachineFreeLook>();

            if(!freeLookCamera)
            {
                Debug.LogError("No CinemachineFreeLook component on freeCamera");
            }
        }

        private void Start()
        {
            freeLookCamera.m_XAxis.Value = 0f;
            freeLookCamera.m_YAxis.Value = 0f;
            cameraDefaultForward = cameraTransform.forward;
            cameraDefaultRight = cameraTransform.right;
            cameraDefaultUp = cameraTransform.up;
        }

        private void FixedUpdate()
        {
            var fwd = cameraTransform.forward;
            var tmp = new Vector3(fwd.x, 0f, fwd.z);
            //Debug.Log($"angle (w/o y) = {Vector3.Angle(cameraDefaultForward, tmp)} {Vector3.Angle(cameraDefaultRight, tmp)}");
            //Debug.Log($"angle (proj w/o y) = {Vector3.Angle(Vector3.ProjectOnPlane(cameraDefaultForward, cameraDefaultUp), tmp)} {Vector3.Angle(Vector3.ProjectOnPlane(cameraDefaultForward, cameraDefaultUp), tmp)}");
            //Debug.Log($"angle (proj) = {Vector3.Angle(Vector3.ProjectOnPlane(cameraDefaultForward, cameraDefaultUp), fwd)} {Vector3.Angle(Vector3.ProjectOnPlane(cameraDefaultForward, cameraDefaultUp), fwd)}");
            //Debug.Log($"angle = {Vector3.Angle(cameraDefaultForward, fwd)} {Vector3.Angle(cameraDefaultRight, fwd)}");
        }

        public void LockOnTarget()
        {
            freeCamera.SetActive(false);
            lockCamera.SetActive(true);
            dialogCamera.SetActive(false);
        }

        public void FreeCamera()
        {
            freeCamera.SetActive(true);
            lockCamera.SetActive(false);
            dialogCamera.SetActive(false);


            //freeLookCamera.transform.position = cameraTransform.position;
            //freeLookCamera.ForceCameraPosition(cameraTransform.position, Quaternion.Euler(Vector3.zero));

            //var forward = cameraTransform.forward;
            //var tmp = new Vector3(forward.x, 0f, forward.z);

            ////var targetAngle = Vector3.Angle(-Vector3.forward, tmp);
            //var targetAngle = Vector3.Angle(cameraDefaultForward, tmp);
            ////float rightAngle = Vector3.Angle(Vector3.right, tmp);
            //float rightAngle = Vector3.Angle(cameraDefaultForward, tmp);

            //freeLookCamera.m_XAxis.Value = rightAngle > 90f ? -targetAngle : targetAngle;
            //freeLookCamera.m_YAxis.Value = 0.5f;
        }

        public void StartDialog()
        {
            freeCamera.SetActive(false);
            lockCamera.SetActive(false);
            dialogCamera.SetActive(true);
        }

        public void OnChangeState(State _, State currentState)
        {
            if (currentState == State.DIALOG)
            {
                StartDialog();
            }
            else if(currentState == State.PEACE) {
                FreeCamera();
            }
        }
    }
}

