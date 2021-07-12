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
                _stats = value;
            }

        }

        private void OnEnable()
        {
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

        private void Start()
        {
            if (!Stats)
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
            if (Stats)
            {
                Stats.OnHealthUpdate -= OnHealthUpdate;
            }
            Stats = null;
        }

        public void OnHealthUpdate(float health, int maxHealth)
        {
            _image.fillAmount = health / (float)maxHealth;
        }

    }
}

