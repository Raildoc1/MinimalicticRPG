using KG.Core;
using UnityEngine;

namespace KG.CameraControl
{
    public class CameraStateController : MonoBehaviour
    {

        [SerializeField] private GameObject freeCamera;
        [SerializeField] private GameObject lockCamera;
        [SerializeField] private GameObject dialogCamera;

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
        }

        public void StartDialog()
        {
            freeCamera.SetActive(false);
            lockCamera.SetActive(false);
            dialogCamera.SetActive(true);
        }

        public void OnChangeState(State state)
        {
            if (state == State.DIALOG)
            {
                StartDialog();
            }
            else {
                FreeCamera();
            }
        }
    }
}

