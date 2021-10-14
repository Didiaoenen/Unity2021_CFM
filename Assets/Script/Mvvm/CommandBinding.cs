using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using mvvm;

public class CommandBinding : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("_view")]
    Component _target;

#pragma warning disable 0169
    [SerializeField]
    [FormerlySerializedAs("_viewEvent")]
    string _targetEvent;

#if !UNITY_EDITOR
#pragma warning restore 0169
#endif

    [SerializeField]
    [FormerlySerializedAs("_viewModel")]
    PropertyBinding.ComponentPath _source = null;

    [SerializeField]
    private BindingParameter _parameter = null;

    public object Parameter
    {
        get 
        {  
            return _runtimeParameter == null ? _parameter.GetValue() : _runtimeParameter; 
        }
        set
        {
            if (_runtimeParameter != null)
            {
                _runtimeParameter = value;
            }
        }
    }

    private object _runtimeParameter = null;

    PropertyBinding.PropertyPath _vmProp;
}

public class BindingParameter
{
    public BindingParameterType Type;
    public UnityEngine.Object ObjectReference;
    public string String;
    public int Int;
    public float Float;
    public bool Bool;

    public object GetValue()
    {
        switch (Type)
        {
            case BindingParameterType.String:
                return String;
            case BindingParameterType.Int:  
                return Int;
            case BindingParameterType.Float:
                return Float;
            case BindingParameterType.Bool:
                return Bool;
            case BindingParameterType.ObjectReference:
                return ObjectReference;
            default:
                return null;
        }
    }
}

public enum BindingParameterType
{
    None,
    ObjectReference,
    String,
    Int,
    Float,
    Bool,
}
