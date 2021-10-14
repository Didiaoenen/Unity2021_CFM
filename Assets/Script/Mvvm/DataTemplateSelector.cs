using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataTemplateSelector : ScriptableObject, IDataTemplateSelector
{
    public abstract GameObject SelectTemplate(object data);
}
