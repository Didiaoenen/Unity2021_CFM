using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBoxItem : Selectable
{
    public bool IsSelected()
    {
        return currentSelectionState == SelectionState.Highlighted || currentSelectionState == SelectionState.Pressed;
    }
}
