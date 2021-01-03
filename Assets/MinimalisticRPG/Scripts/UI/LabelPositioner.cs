using KG.Interact;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KG.UI {
    [RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
    public class LabelPositioner : MonoBehaviour {

        private RectTransform rectTransform;
        private TextMeshProUGUI textMesh;
        private Transform currentTargetLabelPosition;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if (!currentTargetLabelPosition) return;

            UpdateLabelPosition();
        }

        private void UpdateLabelPosition() {
            var pos = Camera.main.WorldToScreenPoint(currentTargetLabelPosition.position);

            var width = Screen.width;
            var height = Screen.height;
            float width_persent = 0.1f;
            float height_persent = 0.01f;
            pos.x = Mathf.Clamp(pos.x, width * width_persent, width * (1 - width_persent));
            pos.y = Mathf.Clamp(pos.y, height * height_persent, height * (1 - height_persent));
            pos.z = 0;

            rectTransform.position = pos;
        }

        public void SetTarget(Interactable target) {

            if (!target) {
                ResetTarget();
                return;
            }

            var labelPos = target.labelPosition;

            if (labelPos) currentTargetLabelPosition = labelPos;
            else currentTargetLabelPosition = target.transform;

            textMesh.text = target.Name;
            UpdateLabelPosition();
            textMesh.enabled = true;

        }

        public void ResetTarget() {
            currentTargetLabelPosition = null;
            textMesh.enabled = false;
        }

        private IEnumerator TestRoutine(Transform transform)
        {
            while (true)
            {
                var pos = transform.position;

                pos.y += Time.deltaTime;

                transform.position = pos;

                yield return null;
            }
        }

    }
}


