using System;
using System.ComponentModel;
using UnityEngine;

namespace Mvvm
{
    public enum BindingMode
    {
        OneTime,
        OneWayToTarget,
        OneWayToSource,
        TwoWay,

        [Obsolete("OneWayToTarget")]
        [HideInInspector]
        [EditorBrowsable(EditorBrowsableState.Never)]
        OneWayToView = OneWayToTarget,

        [Obsolete("OneWayToSource")]
        [HideInInspector]
        [EditorBrowsable(EditorBrowsableState.Never)]
        OneWayToViewModel = OneWayToSource,
    }

    public static class BindingModeExtensions
    {
        public static bool IsTargetBoundToSource(this BindingMode bindingMode)
        {
            return bindingMode == BindingMode.OneWayToTarget || bindingMode == BindingMode.TwoWay;
        }

        public static bool IsSourceBoundToTarget(this BindingMode bindingMode)
        {
            return bindingMode == BindingMode.OneWayToSource || bindingMode == BindingMode.TwoWay;
        }
    }
}
