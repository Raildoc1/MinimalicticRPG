using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KG.Stats
{
    public class BarView : MonoBehaviour
    {

        public Image image;
        [SerializeField] private StatsHolder _stats;
        public StatsHolder stats
        {
            get
            {
                return _stats;
            }
            set
            {
                if (stats)
                {
                    stats.OnHealthUpdate.RemoveListener(OnHealthUpdate);
                }

                _stats = value;

                if (stats)
                {
                    stats.OnHealthUpdate.AddListener(OnHealthUpdate);
                    OnHealthUpdate(stats.Health, stats.MaxHealth);
                }
                else 
                {
                    Disable();
                }

            }

        }

        private void Start()
        {
            if (stats)
            {
                stats.OnHealthUpdate.AddListener(OnHealthUpdate);
                OnHealthUpdate(stats.Health, stats.MaxHealth);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            stats = null;
        }

        public void OnHealthUpdate(int health, int maxHealth)
        {
            image.fillAmount = health / (float)maxHealth;
        }

    }
}

