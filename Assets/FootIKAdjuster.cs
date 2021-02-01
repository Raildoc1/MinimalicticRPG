using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KG.Movement
{
    [RequireComponent(typeof(Animator))]
    public class FootIKAdjuster : MonoBehaviour
    {

        public bool IKEnabled = true;

        public LayerMask layerMask;
        public float groundDistance = 0.25f;
        public float IKOffset = 0.1f;
        public float IKSpeed = 2f;

        private Animator animator;

        private float leftFootOffset = 0f;
        private float rightFootOffset = 0f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {

            if (!IKEnabled || animator.GetFloat("InputMagnitude") > 0.1f) return;

            leftFootOffset = AdjustFootIK(AvatarIKGoal.LeftFoot);
            rightFootOffset = AdjustFootIK(AvatarIKGoal.RightFoot);

            AdjustPelvis();
        }

        private float AdjustFootIK(AvatarIKGoal footIK)
        {
            animator.SetIKPositionWeight(footIK, 1f);
            animator.SetIKRotationWeight(footIK, 1f);

            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(footIK) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, groundDistance + 1, layerMask))
            {
                var result = (animator.GetIKPosition(footIK).y - hit.point.y - IKOffset);
                animator.SetIKPosition(footIK, hit.point + Vector3.up * IKOffset);
                animator.SetIKRotation(footIK, Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation);
                return result;
            }

            return 0f;
        }

        private void AdjustPelvis()
        {
            var offset = leftFootOffset < rightFootOffset ? rightFootOffset : leftFootOffset;

            var newPelvisPosition = animator.bodyPosition - Vector3.up * offset;

            //newPelvisPosition.y = Mathf.MoveTowards(animator.bodyPosition.y, newPelvisPosition.y, IKSpeed * Time.deltaTime);

            animator.bodyPosition = newPelvisPosition;

        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {

            if (!Application.isPlaying)
            {
                return;
            }

            animator = GetComponent<Animator>();

            RaycastHit hit;
            Ray ray = new Ray(animator.GetBoneTransform(HumanBodyBones.LeftFoot).position + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, groundDistance + 1, layerMask))
            {
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
            
            ray = new Ray(animator.GetBoneTransform(HumanBodyBones.RightFoot).position + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, groundDistance + 1, layerMask))
            {
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
        }
#endif
    }
}

