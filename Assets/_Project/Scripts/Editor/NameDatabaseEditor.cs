using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KG.Interact
{
    [CustomEditor(typeof(NameDatabase))]
    public class NameDatabaseEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Sort"))
            {
                var database = (NameDatabase)target;

                database.names.Sort((a, b) => a.unique_name.CompareTo(b.unique_name));
            }
            else if (GUILayout.Button("Save"))
            {
                var database = (NameDatabase)target;
                string json = JsonUtility.ToJson(database, true);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/nameDatabase.json", json);
                Debug.Log($"written to {Application.persistentDataPath + "/nameDatabase.json"}");
            }

        }

    }
}

