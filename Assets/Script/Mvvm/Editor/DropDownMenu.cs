using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DropDownItem
{
    public string Label { get; set; }
    public Action Command { get; set; }
    public bool IsSelected {  get; set; }
}

public class DropDownMenu
{
    private readonly List<DropDownItem> _dropDownItems = new List<DropDownItem>();
    private int _selectedItem = -1;

    public int ItemCount => _dropDownItems.Count;
    public int SelectedIndex => _selectedItem;

    public void Add(DropDownItem item)
    {
        if (item.IsSelected)
        {
            _selectedItem = _dropDownItems.Count;
        }

        _dropDownItems.Add(item);
    }

    public void OnGUI(Rect position)
    {
        OnGUI(dropDownContent => EditorGUI.Popup(position, _selectedItem, dropDownContent));
    }

    public void OnGUI(string label)
    {
        OnGUI(dropDownContent => EditorGUILayout.Popup(new GUIContent(label), _selectedItem, dropDownContent));
    }

    private void OnGUI(Func<GUIContent[], int> showPopup)
    {
        var dropDownContent = new GUIContent[_dropDownItems.Count];
        for (int i = 0;  i < _dropDownItems.Count; i++)
        {
            var item = _dropDownItems[i];

            if (item.Label == null)
            {
                dropDownContent[i] = new GUIContent();
            }
            else
            {
                dropDownContent[i] = new GUIContent(item.Label);
            }
        }

        var clickedIndex = showPopup(dropDownContent);

        if (clickedIndex != _selectedItem)
        {
            _dropDownItems[clickedIndex].Command?.Invoke();
        }
    }
}
