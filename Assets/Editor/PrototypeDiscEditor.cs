using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrototypeDict))]
public class PrototypeDiscEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PrototypeDict script = (PrototypeDict)target;
        DrawDefaultInspector();
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_prototypes"),true);
        if (GUILayout.Button("Parse"))
        {
            script.ParseValidNeighbors();
        }
        if (GUILayout.Button("Add"))
        {
            script.AddToList();
        }
        //serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }

}
