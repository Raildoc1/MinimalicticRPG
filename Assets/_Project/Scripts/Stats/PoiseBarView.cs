using UnityEngine;
using UnityEngine.UI;

namespace KG.Stats
{
    public class PoiseBarView : MonoBehaviour
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

            _stats.OnPoiseUpdate += OnPoiseUpdate;
        }

        private void OnDisable()
        {
            _stats.OnPoiseUpdate -= OnPoiseUpdate;
        }

        public void OnPoiseUpdate(float health, int maxHealth)
        {
            _image.fillAmount = health / maxHealth;
        }
    }
}

