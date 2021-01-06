using UnityEngine;
using KG.Core;

namespace KG.Movement
{
    [RequireComponent(typeof(AnimatorProxy))]
    public class Mover : MonoBehaviour
    {
        public float stopTime = 0.25f;

        [SerializeField] protected float angleError = .2f;
        [SerializeField] protected float rotationSpeed = 10f;
        protected Vector3 targetDirection;
        protected AnimatorProxy animator;

        protected bool _isStrafing;

        public bool IsStrafing
        {
            get
            {
                return _isStrafing;
            }
            set
            {
                if (!animator) animator = GetComponent<AnimatorProxy>();
                animator.isStrafing = value;
                _isStrafing = value;
            }
        }

        protected virtual void Start()
        {
            targetDirection = transform.forward;
        }
        protected virtual void Update()
        {
            UpdateRotation();
        }
        private void UpdateRotation()
        {
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
