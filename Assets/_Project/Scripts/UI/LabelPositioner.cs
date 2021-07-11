using KG.Interact;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KG.UI
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(RectTransform))]
    public class LabelPositioner : MonoBehaviour {

        private RectTransform _rectTransform;
        private TextMeshProUGUI _textMesh;
        private Transform _currentTargetLabelPosition;
        private PlayerTargetDetector _playerTargetDetector;

        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _playerTargetDetector = FindObjectOfType<PlayerTargetDetector>();
            _playerTargetDetector.OnUpdateTarget += SetTarget;
        }

        private void OnDisable()
        {
            _playerTargetDetector.OnUpdateTarget -= SetTarget;
        }

        private void Update() {
            if (!_currentTargetLabelPosition) return;

            UpdateLabelPosition();
        }

        private void UpdateLabelPosition() {
            var pos = Camera.main.WorldToScreenPoint(_currentTargetLabelPosition.position);

            var width = Screen.width;
            var height = Screen.height;
            float width_persent = 0.1f;
            float height_persent = 0.01f;
            pos.x = Mathf.Clamp(pos.x, width * width_persent, width * (1 - width_persent));
            pos.y = Mathf.Clamp(pos.y, height * height_persent, height * (1 - height_persent));
            pos.z = 0;

            _rectTransform.position = pos;
        }

        public void SetTarget(Interactable target) {

            if (!target) {
                ResetTarget();
                return;
            }

            var labelPos = target.labelPosition;

            if (labelPos) _currentTargetLabelPosition = labelPos;
            else _currentTargetLabelPosition = target.transform;

            _textMesh.text = target.Name;
            UpdateLabelPosition();
            _textMesh.enabled = true;

        }

        public void ResetTarget() {
            _currentTargetLabelPosition = null;
            _textMesh.enabled = false;
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


