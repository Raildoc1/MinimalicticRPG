using KG.Interact;
using KG.Inventory;
using UnityEngine;

namespace KG.Core {
    public class DataLoader : MonoBehaviour {
        private void Awake() {
            //ItemDatabase.Instance.LoadItems(GameSettings.Instance.Language);
            NameDB.Instance.LoadNames(GameSettings.Instance.Language);
        }

    }
}
