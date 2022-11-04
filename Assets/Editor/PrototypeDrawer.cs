using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(Prototype))]
public class PrototypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        //position.Set(0, 0, 500, 600);
        Debug.Log(position.width);
        Debug.Log(position.height);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var amountRect = new Rect(position.x, position.y, 30, position.height + 30);
        var unitRect = new Rect(position.x, position.y + 35, 50, position.height + 30);
        EditorGUI.PropertyField(amountRect, prop.FindPropertyRelative("_id"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, prop.FindPropertyRelative("_prefab"), GUIContent.none);
        
        EditorGUI.indentLevel = indent; 
        EditorGUI.EndProperty();
    }
}
