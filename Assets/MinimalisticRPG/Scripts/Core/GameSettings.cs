using UnityEngine;

namespace KG.Core {
    public class GameSettings : ScriptableObject {
        #region Singleton
            private static GameSettings _instance;
            private const string loadPath = "Settings/GameSettings";
            public static GameSettings Instance {
                get{
                    if(!_instance)
                        _instance = FindObjectOfType<GameSettings>();
                    if(!_instance)
                        _instance = Resources.Load<GameSettings>(loadPath);
                    if(!_instance)
                        Debug.LogError("AllItems has not been found.");

                    return _instance;
                }
                set{_instance = value;}
            }
    #endregion
        [System.Serializable]
        public class OnLanguageChangeEvent : UnityEngine.Events.UnityEvent<string> {}
        public OnLanguageChangeEvent onLanguageChange;

        [SerializeField] private string _language = "EN";
        public string Language {
            get{return _language;}
            set{
                _language = value;
                onLanguageChange.Invoke(_language);
            }
        }

    }
}


