using KG.UI;
using TMPro;
using UnityEngine;

namespace KG.Stats
{
    public class StatsView : MonoBehaviour
    {
        private PlayerStatsHolder _target;
        private InventoryGridUI _inventoryGridUI;

        [SerializeField] private TextMeshProUGUI _statNamesTMP;
        [SerializeField] private TextMeshProUGUI _statValuesTMP;

        private void Awake()
        {
            _target = FindObjectOfType<PlayerStatsHolder>();
            _inventoryGridUI = FindObjectOfType<InventoryGridUI>();

        }

        private void OnEnable()
        {
            _inventoryGridUI.OnUpdateUI += UpdateUI;
            UpdateUI();
        }

        private void OnDisable()
        {
            _inventoryGridUI.OnUpdateUI -= UpdateUI;
        }

        public void UpdateUI()
        {
            var statNames = "";

            statNames += "Experience\n";
            statNames += "Health\n";
            statNames += "Strendth\n";
            statNames += "Dexterity\n";
            statNames += "Intelligence\n";

            _statNamesTMP.text = statNames;

            var statValues = "";

            statValues += _target.Experience + "\n";
            statValues += _target.Health.ToString() + " / " + _target.MaxHealth + "\n";
            statValues += _target.Strength + "\n";
            statValues += _target.Dexterity + "\n";
            statValues += _target.Intelligence + "\n";

            _statValuesTMP.text = statValues;
        }

    }
}

