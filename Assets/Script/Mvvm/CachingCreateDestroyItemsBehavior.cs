using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mvvm
{
    public class CachingCreateDestroyItemsBehavior : ICreateDestroyItems
    {
        private readonly List<GameObject> _gameObjectCached = new List<GameObject>();

        private readonly ICreateDestroyItems _createDestroyImpl;

        private readonly Dictionary<object, GameObject> _itemToGameObjectMap = new Dictionary<object, GameObject>();

        public CachingCreateDestroyItemsBehavior(ICreateDestroyItems createDestroyInfo, int initialPoolSize)
        {
            _createDestroyImpl = createDestroyInfo;

            for (int i = 0; i < initialPoolSize; i++)
            {
                var newGameObject = InstantiateItem();
                newGameObject.SetActive(false);
                _gameObjectCached.Add(newGameObject);
            }
        }

        public GameObject CreateItemControl(object item)
        {
            GameObject control = GetCachedControl();
            if (control != null)
            {
                control.SetActive(true);
            }
            else
            {
                control = InstantiateItem();
                _gameObjectCached.Add(control);
            }

            _itemToGameObjectMap.Add(item, control);

            return control;
        }

        public GameObject GetCachedControl()
        {
            GameObject control = null;

            foreach (var go in _gameObjectCached)
            {
                if (!_itemToGameObjectMap.ContainsValue(go))
                {
                    control = go;
                    break;
                }
            }

            return control;
        }

        public void DestroyItemControl(ItemsControl.ItemInfo item)
        {
            item.Control.SetActive(false);
            _itemToGameObjectMap.Remove(item.Item);
        }

        public GameObject InstantiateItem()
        {
            return (_createDestroyImpl.InstantiateItem());
        }

        public void SetItemTemplate(GameObject itemTemplate)
        {
            _createDestroyImpl.SetItemTemplate(itemTemplate);
        }

        public void SetItemTemplate(DataTemplateSelector dataTemplateSelector)
        {
            _createDestroyImpl.SetItemTemplate(dataTemplateSelector);
        }
    }
}
