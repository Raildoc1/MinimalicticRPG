using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KG.Stats
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private StatsHolder _stats;

        public StatsHolder Stats
        {
            get
            {
                return _stats;
            }
            set
            {
                if (Stats)
                {
                    Stats.OnHealthUpdate += OnHealthUpdate;
                }

                _stats = value;

                if (Stats)
                {
                    Stats.OnHealthUpdate += OnHealthUpdate;
                    OnHealthUpdate(Stats.Health, Stats.MaxHealth);
                }
                else 
                {
                    Disable();
                }

            }

        }

        private void Awake()
        {
            
        }

        private void Start()
        {
            if (Stats)
            {
                Stats.OnHealthUpdate -= OnHealthUpdate;
                OnHealthUpdate(Stats.Health, Stats.MaxHealth);
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
            Stats = null;
        }

        public void OnHealthUpdate(float health, int maxHealth)
        {
            _image.fillAmount = health / maxHealth;
        }

    }
}

