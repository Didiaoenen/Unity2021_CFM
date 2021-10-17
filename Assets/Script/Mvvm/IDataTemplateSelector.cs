using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mvvm
{
    public interface IDataTemplateSelector
    {
        GameObject SelectTemplate(object data);
    }
}
