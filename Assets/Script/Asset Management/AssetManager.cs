using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Mvvm
{
    public static class AssetManager
    {
        const string _baseErr = "";

        public static readonly int initInstanceCount = 20;

        public static readonly int initGameObjectCount = 20;

        public static readonly int initLoadingAssetsCount = 20;

        public static readonly int initLoadedAssetsCount = 20;

        public delegate void DelegateAssetLoaded(object key, AsyncOperationHandle handle);

        public static event DelegateAssetLoaded OnAssetLoaded;

        public delegate void DelegateAssetUnloaded(object runtimeKey);

        public static event DelegateAssetUnloaded OnAssetUnloaded;

        static readonly Dictionary<object, AsyncOperationHandle> _loadingAssets = new Dictionary<object, AsyncOperationHandle>(initLoadingAssetsCount);

        static readonly Dictionary<object, AsyncOperationHandle> _loadedAssets = new Dictionary<object, AsyncOperationHandle>(initLoadedAssetsCount);

        public static IReadOnlyList<object> LoadedAssets => _loadedAssets.Values.Select(x => x.Result).ToList();

        static readonly Dictionary<object, List<GameObject>> _instantiatedObjects = new Dictionary<object, List<GameObject>>(initInstanceCount);

        public static int loadedAssetsCount => _loadedAssets.Count;

        public static int loadingAssetCount => _loadingAssets.Count;

        public static int insatantedAssetsCount => _instantiatedObjects.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsLoaded(string key)
        {
            return _loadedAssets.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static bool IsLoaded(object key)
        {
            return _loadedAssets.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsLoading(string key)
        {
            return _loadingAssets.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static bool IsLoading(object key)
        {
            return _loadingAssets.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsInstantiated(string key)
        {
            return _instantiatedObjects.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsInstantiated(object key)
        {
            return _instantiatedObjects.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int InstantiatedCount(string key)
        {
            return !IsInstantiated(key) ? 0 : _instantiatedObjects[key].Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="key"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool TryGetOrLoadObjectAsync<TObjectType>(string key, out AsyncOperationHandle<TObjectType> handle)
            where TObjectType : Object
        {
            if (_loadedAssets.ContainsKey(key))
            {
                try
                {
                    handle = _loadedAssets[key].Convert<TObjectType>();
                }
                catch
                {
                    handle = Addressables.ResourceManager.CreateCompletedOperation(_loadedAssets[key].Result as TObjectType, string.Empty);
                }

                return true;
            }

            if (_loadingAssets.ContainsKey(key))
            {
                try
                {
                    handle = _loadingAssets[key].Convert<TObjectType>();
                }
                catch
                {
                    handle = Addressables.ResourceManager.CreateChainOperation(_loadingAssets[key], chainOp =>
                    {
                        var objectType = chainOp.Result as TObjectType;
                        return Addressables.ResourceManager.CreateCompletedOperation(objectType, string.Empty);
                    });
                }

                return false;
            }

            handle = Addressables.LoadAssetAsync<TObjectType>(key);

            _loadingAssets.Add(key, handle);

            handle.Completed += op2 =>
            {
                _loadedAssets.Add(key, op2);
                _loadingAssets.Remove(key);

                OnAssetLoaded?.Invoke(key, op2);
            };

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool TryGetOrLoadComponentAsync<TComponentType>(string key, out AsyncOperationHandle<TComponentType> handle)
            where TComponentType : Component
        {
            if (_loadedAssets.ContainsKey(key))
            {
                handle = ConvertHandleToComponent<TComponentType>(_loadedAssets[key]);
                return true;
            }

            if (_loadingAssets.ContainsKey(key))
            {
                handle = Addressables.ResourceManager.CreateChainOperation(_loadingAssets[key], ConvertHandleToComponent<TComponentType>);
                return false;
            }

            var op = Addressables.LoadAssetAsync<GameObject>(key);

            _loadingAssets.Add(key, op);

            op.Completed += op2 =>
            {
                _loadedAssets.Add(key, op2);
                _loadingAssets.Remove(key);

                OnAssetLoaded?.Invoke(key, op2);
            };

            handle = Addressables.ResourceManager.CreateChainOperation(op, chainOp =>
            {
                var comp = chainOp.Result.GetComponent<TComponentType>();
                return Addressables.ResourceManager.CreateCompletedOperation(comp, string.Empty);
            });

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetObjectSync<TObjectType>(string key, out TObjectType result)
            where TObjectType : Object
        {
            if (_loadedAssets.ContainsKey(key))
            {
                result = _loadedAssets[key].Convert<TObjectType>().Result;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetComponentSync<TComponentType>(string key, out TComponentType result)
            where TComponentType : Component
        {
            if (_loadedAssets.ContainsKey(key))
            {
                var handle = _loadedAssets[key];
                result = null;
                var go = handle.Result as GameObject;
                if (!go)
                {
                    throw new ConversionException($"Cannot convert {nameof(handle.Result)} to {nameof(GameObject)}.");
                }
                result = go.GetComponent<TComponentType>();
                if (!result)
                {
                    throw new ConversionException($"Cannot {nameof(go.GetComponent)} of Type {typeof(TComponentType)}.");
                }
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<List<AsyncOperationHandle<TObject>>> LoadAssetsByLabelAsync<TObject>(string label)
            where TObject : Object
        {
            var handle = Addressables.ResourceManager.StartOperation(new LoadAssetsByLabelOperation<TObject>(_loadedAssets, _loadingAssets, label, AssetLoadedCallback), default);
            return handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handle"></param>
        static void AssetLoadedCallback(object key, AsyncOperationHandle handle)
        {
            OnAssetLoaded?.Invoke(key, handle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void Unload(object key)
        {
            AsyncOperationHandle handle;
            if (_loadingAssets.ContainsKey(key))
            {
                handle = _loadingAssets[key];
                _loadingAssets.Remove(key);
            }
            else if (_loadedAssets.ContainsKey(key))
            {
                handle = _loadedAssets[key];
                _loadedAssets.Remove(key);
            }
            else
            {
                Debug.LogWarning($"{_baseErr}Cannot {nameof(Unload)} RuntimeKey '{key}': It is not loading or loaded.");
                return;
            }

            if (IsInstantiated(key))
            {
                DestroyAllInstances(key);
            }

            Addressables.Release(handle);

            OnAssetUnloaded?.Invoke(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        public static void UnloadByLabel(string label)
        {
            if (string.IsNullOrEmpty(label) || string.IsNullOrWhiteSpace(label))
            {
                Debug.LogError("Label cannot be empty.");
                return;
            }

            var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
            locationsHandle.Completed += op =>
            {
                if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Cannot Unload by label '{label}'");
                    return;
                }
                foreach (var resourceLocation in op.Result)
                {
                    if (IsLoaded(resourceLocation.PrimaryKey) || IsLoading(resourceLocation.PrimaryKey))
                    {
                        Unload(resourceLocation.PrimaryKey);
                    }
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool TryInstantiateOrLoadAsync(string key, Vector3 postion, Quaternion rotation, Transform parent, out AsyncOperationHandle<GameObject> handle)
        {
            if (TryGetOrLoadObjectAsync(key, out AsyncOperationHandle<GameObject> loadHandle))
            {
                var instance = InstantiateInternal(key, loadHandle.Result, postion, rotation, parent);
                handle = Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<GameObject>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var instance = InstantiateInternal(key, chainOp.Result, postion, rotation, parent);
                return Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
            });

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool TryInstantiateOrLoadAsync<TComponentType>(string key, Vector3 position, Quaternion rotation, Transform parent, out AsyncOperationHandle<TComponentType> handle)
            where TComponentType : Component
        {
            if (TryGetOrLoadComponentAsync(key, out AsyncOperationHandle<TComponentType> loadHandle))
            {
                var instance = InstantiateInternal(key, loadHandle.Result, position, rotation, parent);
                handle = Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<TComponentType>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            //Create a chain that waits for loadHandle to finish, then instantiates and returns the instance GO.
            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var instance = InstantiateInternal(key, chainOp.Result, position, rotation, parent);
                return Addressables.ResourceManager.CreateCompletedOperation(instance, string.Empty);
            });
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool TryInstantiateMultiOrLoadAsync(string key, int count, Vector3 position, Quaternion rotation, Transform parent,
            out AsyncOperationHandle<List<GameObject>> handle)
        {
            if (TryGetOrLoadObjectAsync(key, out AsyncOperationHandle<GameObject> loadHandle))
            {
                var list = new List<GameObject>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(key, loadHandle.Result, position, rotation, parent);
                    list.Add(instance);
                }

                handle = Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<List<GameObject>>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var list = new List<GameObject>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(key, chainOp.Result, position, rotation, parent);
                    list.Add(instance);
                }

                return Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
            });

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static bool TryInstantiateMultiOrLoadAsync<TComponentType>(string key, int count, Vector3 position, Quaternion rotation, Transform parent, out AsyncOperationHandle<List<TComponentType>> handle)
            where TComponentType : Component
        {
            if (TryGetOrLoadComponentAsync(key, out AsyncOperationHandle<TComponentType> loadHandle))
            {
                var list = new List<TComponentType>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(key, loadHandle.Result, position, rotation, parent);
                    list.Add(instance);
                }

                handle = Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
                return true;
            }

            if (!loadHandle.IsValid())
            {
                Debug.LogError($"Load Operation was invalid: {loadHandle}.");
                handle = Addressables.ResourceManager.CreateCompletedOperation<List<TComponentType>>(null, $"Load Operation was invalid: {loadHandle}.");
                return false;
            }

            handle = Addressables.ResourceManager.CreateChainOperation(loadHandle, chainOp =>
            {
                var list = new List<TComponentType>(count);
                for (int i = 0; i < count; i++)
                {
                    var instance = InstantiateInternal(key, chainOp.Result, position, rotation, parent);
                    list.Add(instance);
                }

                return Addressables.ResourceManager.CreateCompletedOperation(list, string.Empty);
            });
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryInstantiateSync(string key, Vector3 position, Quaternion rotation, Transform parent, out GameObject result)
        {
            if (!TryGetObjectSync(key, out GameObject loadResult))
            {
                result = null;
                return false;
            }

            result = InstantiateInternal(key, loadResult, position, rotation, parent);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryInstantiateSync<TComponentType>(string key, Vector3 position, Quaternion rotation, Transform parent,
        out TComponentType result) where TComponentType : Component
        {
            if (!TryGetComponentSync(key, out TComponentType loadResult))
            {
                result = null;
                return false;
            }

            result = InstantiateInternal(key, loadResult, position, rotation, parent);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryInstantiateMultiSync(string key, int count, Vector3 position, Quaternion rotation, Transform parent,
            out List<GameObject> result)
        {
            if (!TryGetObjectSync(key, out GameObject loadResult))
            {
                result = null;
                return false;
            }

            var list = new List<GameObject>(count);
            for (int i = 0; i < count; i++)
            {
                var instance = InstantiateInternal(key, loadResult, position, rotation, parent);
                list.Add(instance);
            }

            result = list;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryInstantiateMultiSync<TComponentType>(string key, int count, Vector3 position, Quaternion rotation, Transform parent, out List<TComponentType> result)
            where TComponentType : Component
        {
            if (!TryGetComponentSync(key, out TComponentType loadResult))
            {
                result = null;
                return false;
            }

            var list = new List<TComponentType>(count);
            for (int i = 0; i < count; i++)
            {
                var instance = InstantiateInternal(key, loadResult, position, rotation, parent);
                list.Add(instance);
            }

            result = list;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="tComponent"></param>
        /// <param name="outHandler"></param>
        public static void TryTakeCompletedOperation<TObjectType>(TObjectType tComponent, out AsyncOperationHandle<TObjectType> outHandler)
            where TObjectType : Component
        {
            outHandler = Addressables.ResourceManager.CreateCompletedOperation(tComponent, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="tComponent"></param>
        /// <param name="handle"></param>
        /// <param name="outHandler"></param>
        public static void TryTakeChainOperation<TObjectType>(TObjectType tComponent, AsyncOperationHandle<TObjectType> handle, out AsyncOperationHandle<TObjectType> outHandler)
            where TObjectType : Component
        {
            outHandler = Addressables.ResourceManager.CreateChainOperation(handle, chainOp =>
            {
                return Addressables.ResourceManager.CreateCompletedOperation(tComponent, string.Empty);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loadedAsset"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        static GameObject InstantiateInternal(string key, GameObject loadedAsset, Vector3 position, Quaternion rotation, Transform parent)
        {
            var instance = Object.Instantiate(loadedAsset, position, rotation, parent);
            if (!instance)
            {
                throw new NullReferenceException($"Instantiated Object of type '{typeof(GameObject)}' is null.");
            }

            var monoTracker = instance.gameObject.AddComponent<MonoTracker>();
            monoTracker.key = key;
            monoTracker.OnDestroyed += TrackerDestroyed;

            if (!_instantiatedObjects.ContainsKey(key))
                _instantiatedObjects.Add(key, new List<GameObject>(initGameObjectCount));

            _instantiatedObjects[key].Add(instance);
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="key"></param>
        /// <param name="loadedAsset"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        static TComponentType InstantiateInternal<TComponentType>(string key, TComponentType loadedAsset, Vector3 position, Quaternion rotation, Transform parent)
            where TComponentType : Component
        {
            var instance = Object.Instantiate(loadedAsset, position, rotation, parent);
            if (!instance)
                throw new NullReferenceException($"Instantiated Object of type '{typeof(TComponentType)}' is null.");

            var monoTracker = instance.gameObject.AddComponent<MonoTracker>();
            monoTracker.key = key;
            monoTracker.OnDestroyed += TrackerDestroyed;

            if (!_instantiatedObjects.ContainsKey(key))
                _instantiatedObjects.Add(key, new List<GameObject>(20));
            _instantiatedObjects[key].Add(instance.gameObject);
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracker"></param>
        static void TrackerDestroyed(MonoTracker tracker)
        {
            if (_instantiatedObjects.TryGetValue(tracker.key, out var list))
            {
                list.Remove(tracker.gameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public static void DestroyAllInstances(string key)
        {
            if (_instantiatedObjects.ContainsKey(key))
            {
                Debug.LogWarning($"'{key}' has not been instantiated. 0 Instances destroyed.");
                return;
            }

            DestroyAllInstances(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        static void DestroyAllInstances(object key)
        {
            var instanceList = _instantiatedObjects[key];
            for (int i = instanceList.Count - 1; i >= 0; i--)
            {
                DestroyInternal(instanceList[i]);
            }

            _instantiatedObjects[key].Clear();
            _instantiatedObjects.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        static void DestroyInternal(Object obj)
        {
            var c = obj as Component;
            if (c)
                Object.Destroy(c.gameObject);
            else
            {
                var go = obj as GameObject;
                if (go)
                    Object.Destroy(go);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponentType"></typeparam>
        /// <param name="handle"></param>
        /// <returns></returns>
        static AsyncOperationHandle<TComponentType> ConvertHandleToComponent<TComponentType>(AsyncOperationHandle handle)
            where TComponentType : Component
        {
            GameObject go = handle.Result as GameObject;
            if (!go)
            {
                throw new ConversionException($"Cannot convert {nameof(handle.Result)} to {nameof(GameObject)}.");
            }
            TComponentType comp = go.GetComponent<TComponentType>();
            if (!comp)
            {
                throw new ConversionException($"Cannot {nameof(go.GetComponent)} of Type {typeof(TComponentType)}.");
            }
            var result = Addressables.ResourceManager.CreateCompletedOperation(comp, string.Empty);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        static List<object> GetKeysFromLocations(IList<IResourceLocation> locations)
        {
            List<object> keys = new List<object>(locations.Count);

            foreach (var locator in Addressables.ResourceLocators)
            {
                foreach (var key in locator.Keys)
                {
                    bool isGUID = Guid.TryParse(key.ToString(), out var guid);
                    if (!isGUID)
                    {
                        continue;
                    }

                    if (!TryGetKeyLocationID(locator, key, out var keyLocationID))
                    {
                        continue;
                    }

                    var locationMatched = locations.Select(x => x.InternalId).ToList().Exists(x => x == keyLocationID);
                    if (!locationMatched)
                    {
                        continue;
                    }

                    keys.Add(key);
                }
            }

            return keys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="key"></param>
        /// <param name="internalID"></param>
        /// <returns></returns>
        static bool TryGetKeyLocationID(IResourceLocator locator, object key, out string internalID)
        {
            internalID = string.Empty;
            var hasLocation = locator.Locate(key, typeof(Object), out var keyLocations);
            if (!hasLocation)
            {
                return false;
            }
            if (keyLocations.Count == 0)
            {
                return false;
            }
            if (keyLocations.Count > 1)
            {
                return false;
            }

            internalID = keyLocations[0].InternalId;
            return true;
        }
    }
}
