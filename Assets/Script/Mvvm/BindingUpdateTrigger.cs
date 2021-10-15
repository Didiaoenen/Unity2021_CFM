using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mvvm
{
    public enum BindingUpdateTrigger
    {
        None,
        PropertyChangedEvent,
        UnityEvent,
    }
}
