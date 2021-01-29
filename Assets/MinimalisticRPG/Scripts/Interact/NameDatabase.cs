using KG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KG.Interact
{
    [CreateAssetMenu]
    public class NameDatabase : ScriptableObject
    {

        #region Singleton
        private static NameDatabase _instance;
        private const string loadPath = "Names/NameDatabase";
        public static NameDatabase Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.Load<NameDatabase>(loadPath);
                if (!_instance)
                    Debug.LogError("AllItems has not been found.");

                return _instance;
            }
            set { _instance = value; }
        }
        #endregion

        [System.Serializable]
        public struct NameDatabaseEntity
        {
            public string unique_name;
            public List<LanguageVariant> display_name;
        }

        [System.Serializable]
        public struct LanguageVariant
        {
            public string language;
            public string name;
        }

        public List<NameDatabaseEntity> names;

        public string GetName(string uniqueName, string language)
        {

            var unHash = uniqueName.GetHashCode();
            var lHash = language.GetHashCode();

            foreach (var entity in names)
            {
                if (entity.unique_name.GetHashCode() == unHash)
                {
                    foreach (var variant in entity.display_name)
                    {
                        if (variant.language.GetHashCode() == lHash)
                        {
                            return variant.name;
                        }
                    }
                    return "no_name";
                }
            }
            return "no_name";
        }

        public string GetName(string uniqueName)
        {
            return GetName(uniqueName, GameSettings.Instance.Language);
        }
    }

}
