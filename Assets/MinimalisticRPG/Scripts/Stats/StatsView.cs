using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KG.Stats
{
    public class StatsView : MonoBehaviour
    {

        public PlayerStatsHolder target;

        public TextMeshProUGUI statNamesTMP;
        public TextMeshProUGUI statValuesTMP;

        private void OnEnable()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            var statNames = "";

            statNames += "Experience\n";
            statNames += "Health\n";
            statNames += "Strendth\n";
            statNames += "Dexterity\n";
            statNames += "Intelligence\n";

            statNamesTMP.text = statNames;

            var statValues = "";

            statValues += target.Experience + "\n";
            statValues += target.Health.ToString() + " / " + target.MaxHealth + "\n";
            statValues += target.Strength + "\n";
            statValues += target.Dexterity + "\n";
            statValues += target.Intelligence + "\n";

            statValuesTMP.text = statValues;
        }

    }
}

