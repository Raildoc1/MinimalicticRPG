using KG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Interact
{
    [RequireComponent(typeof(StateSwitch))]
    public class PlayerTargetDetector : MonoBehaviour
    {

        [SerializeField] private float maxDistance = 6f;
        [SerializeField] private List<string> tags_to_detect;

        #region Event
        [System.Serializable]
        public class UpdateTargetEvent : UnityEngine.Events.UnityEvent<Interactable> { }
        public UpdateTargetEvent OnUpdateTarget;
        #endregion

        public Interactable current_target { get; private set; } = null;

        private bool _fixedTarget = false;
        private StateSwitch stateSwitch;

        private void Awake()
        {
            stateSwitch = GetComponent<StateSwitch>();
        }

        public void Interact()
        {
            if (stateSwitch.CurrentState != State.PEACE || !current_target)
            {
                return;
            }

            if (current_target is Talkable)
            {
                stateSwitch.CurrentState = State.DIALOG;
            }

            current_target.Interact();
        }

        private void Update()
        {
            if (_fixedTarget) return;
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, maxDistance);
            var new_target = GetNewTarget(hits);

            if (new_target == null || new_target != current_target)
            {
                if (OnUpdateTarget != null) OnUpdateTarget.Invoke(new_target);
                current_target = new_target;
            }
        }

        private bool IsRightTag(string tag)
        {
            foreach (string s in tags_to_detect)
            {
                if (string.Equals(tag, s)) return true;
            }
            return false;
        }

        private Interactable GetNewTarget(RaycastHit[] hits)
        {
            foreach (RaycastHit hit in hits)
            {
                Interactable i = hit.collider.gameObject.GetComponent<Interactable>();
                if (i)
                    if (IsRightTag(i.tag))
                        return i;
            }
            return null;
        }
    }
}
