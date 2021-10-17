using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mvvm
{
    public class ListBox : Selector
    {
        private GameObject _lastSelected;
        private static readonly Func<ListBoxItem, bool> _selectionState = s => s == null ? false : s.IsSelected();

        void LateUpdate()
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected == _lastSelected) return;

            _lastSelected = selected;

            if (selected == null) return;
            if (selected.transform.GetComponentInParent<Selector>() == null) return;

            if (SelectedInfo != null && SelectedInfo.Control != null)
            {
                if (_selectionState(SelectedInfo.Control.GetComponent<ListBoxItem>()))
                        return;
            }

            SetSelected(GetItemInfo(selected));
        }

        private ItemInfo GetItemInfo(GameObject selected)
        {
            if (selected == null) return null; 

            foreach (var info in _items)
            {
                if (info.Control == selected) 
                    return info;
            }

            var parentItem = selected.GetComponentInParent<ListBoxItem>();
            var parent = parentItem == null ? null : parentItem.gameObject;
            foreach (var info in _items)
            {
                if (info.Control == parent)
                    return info;
            }

            return null;
        }

        protected override void OnSelectedChanged(bool fromProperty)
        {
            if (!fromProperty)
            {
                if (SelectedInfo != null)
                    EventSystem.current.SetSelectedGameObject(SelectedInfo.Control);
                else
                    EventSystem.current.SetSelectedGameObject(null);
            }

            base.OnSelectedChanged(fromProperty);
        }
    }
}
