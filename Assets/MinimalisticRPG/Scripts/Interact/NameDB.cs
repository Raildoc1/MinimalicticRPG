using KG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KG.Interact {
    [CreateAssetMenu]
    public class NameDB : ScriptableObject {

        #region Singleton
        private static NameDB _instance;
        private const string loadPath = "Names/NameDB";
        public static NameDB Instance {
            get {
                if (!_instance)
                    _instance = Resources.Load<NameDB>(loadPath);
                if (!_instance)
                    Debug.LogError("AllItems has not been found.");

                return _instance;
            }
            set { _instance = value; }
        }
        #endregion

        [SerializeField]
        private NameDBAsset _names;

        private NameDictionaryEntity nameDictionaryEntity;

        private NameDBAsset Names {
            get {
                if (_names == null) LoadNames(GameSettings.Instance.Language);
                if (_names == null) Debug.LogError($"Could not load names of {GameSettings.Instance.Language} language.");
                return _names;
            }
            set {
                _names = value;
            }
        }

        public void LoadNames(string LanguageName) {
            Debug.Log($"Loading {LanguageName} names...");
            try {
                string json = Resources.Load<TextAsset>("Names/" + LanguageName + "/names").text;
                Names = JsonUtility.FromJson<NameDBAsset>(json);
                Debug.Log($"{Names.names.Count} {LanguageName} names loaded successfully...");
            } catch (System.NullReferenceException) {
                Debug.Log($"Failed to load {LanguageName} names...");
            }

        }

        public string GetName(string unique_name) {
            foreach (NameDictionaryEntity n in Names.names) {
                if (n.unique_name.Equals(unique_name)) return n.display_name;
            }
            return "NO_NAME";
        }

        [System.Serializable]
        public class NameDictionaryEntity {
            public string unique_name;
            public string display_name;
        }

        [System.Serializable]
        public class NameDBAsset {
            public List<NameDictionaryEntity> names;
        }

#if UNITY_EDITOR
        [MenuItem("Names/Export JSON")]
        public static void SaveItems() {
            Debug.Log($"Names list: {Instance.Names.names.Count}");
            string json = JsonUtility.ToJson(Instance.Names, true);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/names.json", json);
            Debug.Log($"written to {Application.persistentDataPath + "/names.json"}");
        }
#endif

    }
}


