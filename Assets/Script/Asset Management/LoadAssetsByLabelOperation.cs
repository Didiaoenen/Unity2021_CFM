using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Mvvm
{

    public class LoadAssetsByLabelOperation<TObject> : AsyncOperationBase<List<AsyncOperationHandle<TObject>>> where TObject : Object
    {
        string _label;
        Action<object, AsyncOperationHandle> _loadedCallback;
        Dictionary<object, AsyncOperationHandle> _loadedDictionary;
        Dictionary<object, AsyncOperationHandle> _loadingDictionary;

        Dictionary<string, AsyncOperationHandle<TObject>> _loadingInternalIdDic = new Dictionary<string, AsyncOperationHandle<TObject>>();

        public LoadAssetsByLabelOperation(Dictionary<object, AsyncOperationHandle> loadedDictionary, Dictionary<object, AsyncOperationHandle> loadingDictionary,
            string label, Action<object, AsyncOperationHandle> loadedCallback)
        {
            _label = label;
            _loadedCallback = loadedCallback;

            _loadedDictionary = loadedDictionary;
            if (_loadedDictionary == null)
                _loadedDictionary = new Dictionary<object, AsyncOperationHandle>();

            _loadingDictionary = loadingDictionary;
            if (_loadingDictionary == null)
                _loadingDictionary = new Dictionary<object, AsyncOperationHandle>();
        }

        protected override void Execute()
        {
#pragma warning disable CS4014
            DoTask();
#pragma warning restore CS4014
        }

        public async Task DoTask()
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(_label);
            var locations = await locationsHandle.Task;

            var operationHandles = new List<AsyncOperationHandle<TObject>>();
            foreach (var resourceLocation in locations)
            {
                if (resourceLocation.ResourceType == typeof(TObject))
                {
                    AsyncOperationHandle<TObject> loadingHandle = Addressables.LoadAssetAsync<TObject>(resourceLocation.PrimaryKey);

                    operationHandles.Add(loadingHandle);

                    if (!_loadingInternalIdDic.ContainsKey(resourceLocation.PrimaryKey))
                        _loadingInternalIdDic.Add(resourceLocation.PrimaryKey, loadingHandle);

                    loadingHandle.Completed += assetOp =>
                    {
                        if (!_loadingDictionary.ContainsKey(resourceLocation.PrimaryKey))
                            _loadingDictionary.Add(resourceLocation.PrimaryKey, assetOp);
                    };
                }
            }

            foreach (var handle in operationHandles)
                await handle.Task;

            foreach (var handle in _loadingInternalIdDic)
            {
                if (_loadingDictionary.ContainsKey(handle.Key))
                    _loadingDictionary.Remove(handle.Key);

                if (!_loadedDictionary.ContainsKey(handle.Key))
                {
                    _loadedDictionary.Add(handle.Key, handle.Value);
                    _loadedCallback?.Invoke(handle.Key, handle.Value);
                }
            }

            Complete(operationHandles, true, string.Empty);
        }
    }
}
