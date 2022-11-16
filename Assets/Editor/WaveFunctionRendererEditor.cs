using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//[CustomEditor(typeof(WaveFunctionRenderer))]
public class WaveFunctionRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaveFunctionRenderer script = (WaveFunctionRenderer)target;
        //DrawDefaultInspector();
        /*if (GUILayout.Button("Draw"))
        {
            script.DrawWaveFunction();
        }
        if (GUILayout.Button("Clear"))
        {
            script.Clear();
        }
        /*if (GUILayout.Button("Next"))
        {
            script.PrepareNext();
        }*/
        base.OnInspectorGUI();
    }
}
