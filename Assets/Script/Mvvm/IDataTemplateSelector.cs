using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataTemplateSelector
{
    GameObject SelectTemplate(object data);
}
