using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KG.CustomEditors
{
    [CustomEditor(typeof(ItemsList))]
    public class ItemsListEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var list = (ItemsList)target;

            if (GUILayout.Button("Generate hashes"))
            {
                foreach (var item in list.items)
                {
                    if (!item.itemName.Equals(""))
                    {
                        item.hash = item.itemName.GetHashCode();
                    }
                }
            }
            else if (GUILayout.Button("Sort"))
            {
                list.items.Sort((a, b) => a.type.CompareTo(b.type));
            }
            else if (GUILayout.Button("Save"))
            {
                string json = JsonUtility.ToJson(list, true);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/itemList.json", json);
                Debug.Log($"written to {Application.persistentDataPath + "/itemList.json"}");
            }
        }
    }
}

