using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KG.Stats
{
    public class BarView : MonoBehaviour
    {

        public Image image;
        public StatsHolder stats;

        public void Init()
        {
            stats.OnHealthUpdate.AddListener(OnHealthUpdate);
        }

        private void OnEnable()
        {

            if (!stats)
            {
                enabled = false;
                return;
            }

            Init();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (stats)
            {
                stats.OnHealthUpdate.RemoveListener(OnHealthUpdate);
            }

            stats = null;
        }

        public void OnHealthUpdate(int health, int maxHealth)
        {
            image.fillAmount = health / (float)maxHealth;
        }

    }
}

