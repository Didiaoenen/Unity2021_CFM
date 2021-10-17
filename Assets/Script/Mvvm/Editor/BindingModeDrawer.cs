using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Mvvm;

[CustomPropertyDrawer(typeof(BindingMode))]
public class BindingModeDrawer : ObsoleteAwareEnumDrawer<BindingMode>
{

}

public class ObsoleteAwareEnumDrawer<EnumT> : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var dropDownPosition = EditorGUI.PrefixLabel(position, label);

        var enumType = typeof(EnumT);
        var names = Enum.GetNames(enumType);
        var values = Enum.GetValues(enumType).Cast<int>().ToList();

        var selectedItem = property.intValue;
        int newSelectedItem = selectedItem;

        DropDownMenu menu = new DropDownMenu();
        for (int i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = values[i];
            var enumValueMember = enumType.GetMember(name).First();
            var isObsolete = enumValueMember.GetCustomAttributes(typeof(ObsoleteAttribute), false).Any();
            
            if (isObsolete)
            {
                menu.Add(new DropDownItem
                {
                    Label = name,
                    IsSelected = property.intValue == value,
                    Command = () => { property.intValue = value; },
                });
            }

        }
        menu.OnGUI(dropDownPosition);
        
        if (newSelectedItem != selectedItem)
        {
            property.intValue = newSelectedItem;
        }
    }
}
