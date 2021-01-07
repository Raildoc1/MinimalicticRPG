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

            if (GUILayout.Button("Generate hashes"))
            {
                var tar = (ItemsList)target;

                foreach (var item in tar.meleeWeapons)
                {
                    if (!item.itemName.Equals(""))
                    {
                        item.hash = Animator.StringToHash(item.itemName);
                    }
                }
            }
        }
    }
}

