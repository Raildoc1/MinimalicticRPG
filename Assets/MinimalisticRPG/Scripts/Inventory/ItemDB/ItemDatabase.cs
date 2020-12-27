using UnityEngine;
using UnityEditor;
using KG.Core;
using System.Collections.Generic;

namespace KG.Inventory {
    [CreateAssetMenu]
    public class ItemDatabase : ScriptableObject {
        
        #region Singleton
        private static ItemDatabase _instance;
        private const string loadPath = "Items/ItemDatabase";
        public static ItemDatabase Instance {
            get{
                if(!_instance)
                    _instance = Resources.Load<ItemDatabase>(loadPath);
                if(!_instance)
                    Debug.LogError("AllItems has not been found.");

                return _instance;
            }
            set{_instance = value;}
        }
        #endregion
        
        #region PublicFields
        private ItemDatabaseAsset ItemDescriptions {
            get{
                if(_itemDescriptions == null) LoadItems(GameSettings.Instance.Language);
                if(_itemDescriptions == null) Debug.LogError($"Could not load Items of {GameSettings.Instance.Language} language.");
                return _itemDescriptions;
            }
            set{
                _itemDescriptions = value;
            }
        }
        
        private List<Item> Items {
            get{
                if(_items == null) LoadItems(GameSettings.Instance.Language);
                if(_items == null) Debug.LogError($"Could not Load Items of {GameSettings.Instance.Language} language.");
                return _items;
            }
            set{
                _items = value;
            }
        }

        private List<MeleeWeapon> MeleeWeapons {
            get{
                if(_items == null) LoadItems(GameSettings.Instance.Language);
                if(_items == null) Debug.LogError($"Could not Load Items of {GameSettings.Instance.Language} language.");
                return _meleeWeapons;
            }
            set{
                _meleeWeapons = value;
            }
        }

        // Find MeleeWeapon by unique name
        public MeleeWeapon GetWeapon(string name) {
            foreach(MeleeWeapon i in MeleeWeapons) {
                if(i.itemName.Equals(name)) {
                    return i;
                }
            }
            return null;
        }
        #endregion

        #region PrivateFields
        private List<Item> _items;
        private List<MeleeWeapon> _meleeWeapons;
        private ItemDatabaseAsset _itemDescriptions;
        #endregion

        public void LoadItems(string LanguageName) {
            Debug.Log($"Loading {LanguageName} items...");
            try{
                string json = Resources.Load<TextAsset>("Items/Descriptions/" + LanguageName + "/items").text;
                ItemDescriptions = JsonUtility.FromJson<ItemDatabaseAsset>(json);
                Items = new List<Item>(Resources.LoadAll<Item>("Items/Items"));
                Debug.Log($"Items loaded successfully! {Items.Count} item{((Items.Count == 1) ? "" : "s")} found!");
                MeleeWeapons = new List<MeleeWeapon>(Resources.LoadAll<MeleeWeapon>("Items/Items"));
                Debug.Log($"Weapons loaded successfully! {MeleeWeapons.Count} item{((MeleeWeapons.Count == 1) ? "" : "s")} found!");
            } catch (System.NullReferenceException) {
                Debug.Log($"Failed to load {LanguageName} items!");
            }
        }

#if UNITY_EDITOR
        [MenuItem("ItemDB/Export JSON")]
        public static void SaveItems() {
            string json = JsonUtility.ToJson(Instance.ItemDescriptions, true);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/items.json", json);
            Debug.Log($"written to {Application.persistentDataPath + "/items.json"}");
        }
#endif

    }
}
