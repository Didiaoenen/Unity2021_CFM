using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mvvm
{
    public enum BindingUpdateTrigger
    {
        None,
        PropertyChangedEvent,
        UnityEvent,
    }
}
