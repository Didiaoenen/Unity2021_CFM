using UnityEngine;

namespace Mvvm
{
    public class DefaultCreateDestroyItemsBehavior : ICreateDestroyItems
    {
        private GameObject _itemTemplate;
        private DataTemplateSelector _itemTemplateSelector;
        private Transform _itemParent;

        public DefaultCreateDestroyItemsBehavior(Transform parent, GameObject itemTemplate, DataTemplateSelector itemTemplateSelcetor)
        {
            _itemParent = parent;
            _itemTemplate = itemTemplate;
            _itemTemplateSelector = itemTemplateSelcetor;
        }

        public GameObject CreateItemControl(object item)
        {
            CheckItemTemplateSelector(item);

            return InstantiateItem();
        }

        public GameObject InstantiateItem()
        {
            if (_itemTemplate == null)
            {
                throw new System.NotImplementedException("");
            }

            var newGameObject = Object.Instantiate(_itemTemplate);
            var rect = newGameObject.GetComponent<RectTransform>();
            if (rect != null)
                newGameObject.transform.parent = _itemParent;
            else
                rect.SetParent(_itemParent, false);

            return newGameObject;
        }

        public void CheckItemTemplateSelector(object item)
        {
            _itemTemplate = _itemTemplateSelector != null ? _itemTemplateSelector.SelectTemplate(item) : _itemTemplate;
        }

        public void DestroyItemControl(ItemsControl.ItemInfo item)
        {
            var rect = item.Control.GetComponent<RectTransform>();
            if (rect != null)
                item.Control.transform.parent = null;
            else
                rect.SetParent(null, false);

            Object.Destroy(item.Control);
        }

        public void SetItemTemplate(GameObject itemTemplate)
        {
            throw new System.NotImplementedException();
        }

        public void SetItemTemplate(DataTemplateSelector dataTemplateSelector)
        {
            throw new System.NotImplementedException();
        }
    }
}
