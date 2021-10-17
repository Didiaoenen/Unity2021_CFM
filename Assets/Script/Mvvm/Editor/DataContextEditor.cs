using System;
using UnityEditor;
using UnityEngine;
using Mvvm;
using AutoSuggest;

[CustomEditor(typeof(DataContext))]
public class DataContextEditor : Editor
{
    private static readonly string SearchFiledLabel = "Type name";

    private string _searchString = null;
    private Type _tval;
    private TypeSuggestionProvider _suggestionProvider;
    private AutoSuggestField _autoSuggestField;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var tprop = serializedObject.FindProperty("_type");
        var iprop = serializedObject.FindProperty("_instantialOnAwake");
        var bprop = serializedObject.FindProperty("_propertyBinding");

        if (_autoSuggestField == null)
        {
            _suggestionProvider = new TypeSuggestionProvider();
            _autoSuggestField = new AutoSuggestField(
                _suggestionProvider,
                new GUIContent(SearchFiledLabel),
                new AutoSuggestField.Options
                {
                    DisplayMode = DisplayMode.Inline,
                });
        }

        if (_searchString == null)
        {
            _tval = Type.GetType(tprop.stringValue);
            _searchString = _tval?.FullName ?? String.Empty;
        }

        _searchString = _autoSuggestField.OnGUI(_searchString);

        if (_suggestionProvider.SelectedTypeIsValid && Event.current.type == EventType.Layout)
        {
            _tval = _suggestionProvider.SelectedType;
            tprop.stringValue = _tval?.AssemblyQualifiedName;
        }

        if (_tval != null)
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(_tval))
            {
                GUILayout.Label("");
            }    
            else
            {
                EditorGUILayout.PropertyField(iprop);
                EditorGUILayout.PropertyField(bprop);
            }
        }

        serializedObject.ApplyModifiedProperties();

        var dc = target as DataContext;
        if (dc != null)
        {
            using (var disabledGroupScope = new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.Toggle("", dc.Value != null);
            }
        }
    }
}
