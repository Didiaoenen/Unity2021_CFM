using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDelayedValue
{
    bool ValueOrSubscribe(Action<object> whenReady, ref object readyValue);
}
