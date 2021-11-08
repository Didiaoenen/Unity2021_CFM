using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var mode = serializedObject.FindProperty("_mode");

        EditorGUILayout.PropertyField(mode, new GUIContent("enum1"));

        serializedObject.ApplyModifiedProperties();
    }
}
