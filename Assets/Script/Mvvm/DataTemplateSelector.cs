using UnityEngine;

namespace Mvvm
{
    public abstract class DataTemplateSelector : ScriptableObject, IDataTemplateSelector
    {
        public abstract GameObject SelectTemplate(object data);
    }
}
