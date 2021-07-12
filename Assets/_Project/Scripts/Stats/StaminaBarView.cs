using UnityEngine;
using UnityEngine.UI;

namespace KG.Stats
{
    public class StaminaBarView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private StatsHolder _stats;

        private void OnEnable()
        {
            if (!_stats)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                _stats = player.GetComponent<StatsHolder>();
            }

            _stats.OnStaminaUpdate += OnStaminaUpdate;
        }

        private void OnDisable()
        {
            _stats.OnStaminaUpdate -= OnStaminaUpdate;
        }

        public void OnStaminaUpdate(float health, int maxHealth)
        {
            _image.fillAmount = health / maxHealth;
        }
    }
}

