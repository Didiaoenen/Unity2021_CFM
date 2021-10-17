using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateScriptableObjects : ScriptableWizard
{
    private static Type[] _scripTypes;
    private static string[] _names;
    private int _idx;

    [MenuItem("Assets/Create/Scriptable object")]
    public static void CreateWizard()
    {
        if (_scripTypes == null)
        {
            var ueda = typeof(ScriptableWizard).Assembly;
            var editorTypes = ueda.GetTypes().Where(t => typeof(ScriptableObject).IsAssignableFrom(t));

            var validAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !Equals(a, ueda) && !a.FullName.Contains("UnityEditor"));
            _scripTypes = validAssemblies
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch
                    {
                        return Enumerable.Empty<Type>();
                    }
                })
                .Where(t => typeof(ScriptableObject).IsAssignableFrom(t) && !editorTypes.Any(e => e.IsAssignableFrom(t)))
                .ToArray();

            _names = _scripTypes.Select(t => t.FullName).ToArray();
            
            var wizard = DisplayWizard<CreateScriptableObjects>("Create scriptable object");
            wizard._idx = 0;
        }
    }

    protected override bool DrawWizardGUI()
    {
        var idx = EditorGUILayout.Popup(_idx, _names);
        if (idx ==  _idx) return false;
        _idx = idx;
        return true;
    }

    void OnWizardCreate()
    {
        var type = _scripTypes[_idx];
        var asset = ScriptableObject.CreateInstance(type);
        if (asset == null)
        {
            EditorUtility.DisplayDialog("Error", string.Format("{0}", type), "Okey");
            return;
        }
        ProjectWindowUtil.CreateAsset(asset, "New " + type.Name + ".asset");
    }
}
