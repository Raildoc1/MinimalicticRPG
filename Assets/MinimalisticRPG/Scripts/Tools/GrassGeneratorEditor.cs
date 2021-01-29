using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrassGenerator))]
public class GrassGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Draw"))
        {
            var generator = (GrassGenerator)target;

            generator.Draw();
        }

    }

}
