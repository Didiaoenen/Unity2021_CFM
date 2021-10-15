using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mvvm
{
    public interface IDataTemplateSelector
    {
        GameObject SelectTemplate(object data);
    }
}
