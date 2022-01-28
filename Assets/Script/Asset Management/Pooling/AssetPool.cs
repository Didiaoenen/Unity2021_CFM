using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Mvvm
{
    public abstract class AssetPool : IPool, IDisposable
    {
        public const int MinimumCapacityDefault = 5;

        protected static readonly Dictionary<object, AssetPool> AllPools = new Dictionary<object, AssetPool>();

        protected abstract Object loadedObject { get; }

        protected abstract ICollection collection { get; }

        bool _disposed;

        protected bool disposed => _disposed;

        protected bool _isReady;

        public virtual bool isReady => _isReady;

        public readonly string key;

        public readonly int initialCapacity;

        public readonly Transform objectsParent;

        public int minimumCapacity;

        protected AssetPool(string key, int initialCapacity, Transform objectsParent)
        {
            this.key = key;
            this.initialCapacity = initialCapacity;
            this.objectsParent = objectsParent;
            this.minimumCapacity = MinimumCapacityDefault;

            AllPools.Add(key, this);
        }

        ~AssetPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                AllPools.Remove(key);
                _isReady = false;
            }

            _disposed = true;
        }

        public static bool TryGetPool(string key, out AssetPool assetReferencePool)
        {
            return AllPools.TryGetValue(key, out assetReferencePool);
        }

        public static bool TryGetPool(object key, out AssetPool assetReferencePool)
        {
            return AllPools.TryGetValue(key, out assetReferencePool);
        }

        protected abstract void AddToPool(int count);

        public abstract void PoolObjectReturned(PoolObject poolObject);

        public static void LogAllPools()
        {
            foreach (var kvp in AllPools)
            {
                Debug.Log($"{nameof(AssetPool)}: Key {kvp.Key} | Value {kvp.Value} - {kvp.Value.collection.Count} instantiated objects.");
            }
        }
    }

    public class AssetPool<TComponent> : AssetPool where TComponent : Component
    {
        public delegate void DelegatePoolReady();
        public event DelegatePoolReady OnPoolReady;

        public delegate void DelegateObjectInstantiated(TComponent obj);
        public event DelegateObjectInstantiated OnObjectInstantiated;

        protected override Object loadedObject => _loadedObject;

        protected override ICollection collection => _list;

        TComponent _loadedObject;

        List<Tuple<TComponent, PoolObject>> _list;

        AsyncOperationHandle<TComponent> _loadHandle;
        public AsyncOperationHandle<TComponent> loadHandle => _loadHandle;

        Action<TComponent> _objectInstantiatedAction;

        protected AssetPool(string key, int initialCapacity, Transform objectsParent, Action<TComponent> objectInstantiatedAction)
            : base(key, initialCapacity, objectsParent)
        {
            _list = new List<Tuple<TComponent, PoolObject>>(initialCapacity);
            _objectInstantiatedAction = objectInstantiatedAction;

            AssetManager.OnAssetLoaded += OnObjectLoaded;
            AssetManager.OnAssetUnloaded += OnObjectUnloaded;

            if (AssetManager.TryGetOrLoadComponentAsync(key, out _loadHandle))
            {
                OnObjectLoaded(_loadHandle.Result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                AssetManager.OnAssetLoaded -= OnObjectLoaded;
                AssetManager.OnAssetUnloaded -= OnObjectUnloaded;
                _list.Clear();
            }

            base.Dispose(disposing);
        }

        void OnObjectLoaded(object key, AsyncOperationHandle handle)
        {
            GameObject go = handle.Result as GameObject;

            TComponent comp = null;
            if (go)
                comp = go.GetComponent<TComponent>();

            OnObjectLoaded(comp);
        }

        void OnObjectLoaded(TComponent obj)
        {
            _loadedObject = obj;

            _isReady = true;

            AddToPoolSyncSafe(initialCapacity);

            OnPoolReady?.Invoke();
        }

        void OnObjectUnloaded(object runtimeKey)
        {
            if (runtimeKey.ToString() != key)
                return;

            Dispose();
        }

        protected override void AddToPool(int count)
        {
            if (_isReady)
            {
                AddToPoolSyncSafe(count);
                return;
            }
            _loadHandle.Completed += op =>
            {
                AddToPoolSyncSafe(count);
            };
        }

        void AddToPoolSyncSafe(int count)
        {
            var pos = objectsParent ? objectsParent.position : Vector3.zero;

            AssetManager.TryInstantiateMultiSync<TComponent>(key, count, pos, Quaternion.identity, objectsParent, out var instanceList);
            foreach (var component in instanceList)
            {
                var po = component.gameObject.AddComponent<PoolObject>();
                po.myPool = this;
                po.ReturnToPool();

                var tuple = new Tuple<TComponent, PoolObject>(component, po);
                _list.Add(tuple);

                var monoTracker = component.GetComponent<MonoTracker>() ?? component.gameObject.AddComponent<MonoTracker>();
                monoTracker.OnDestroyed += tracker => { _list.Remove(tuple); };

                _objectInstantiatedAction?.Invoke(component);
                OnObjectInstantiated?.Invoke(component);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poolObject"></param>
        public override void PoolObjectReturned(PoolObject poolObject)
        {
            if (objectsParent)
            {
                poolObject.transform.SetParent(objectsParent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool TryTake(Vector3 position, Quaternion rotation, Transform parent, out AsyncOperationHandle<TComponent> handle)
        {
            if (isReady)
            {
                AssetManager.TryTakeCompletedOperation(TakeSync(position, rotation, parent), out handle);
                return true;
            }

            AssetManager.TryTakeChainOperation(TakeSync(position, rotation, parent), _loadHandle, out handle);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TComponent TakeSync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            string errString = $"{GetType()} Error: ";
            if (!isReady)
            {
                Debug.LogError(errString + "not ready.");
                return null;
            }

            var inPoolCount = _list.Count(x => x.Item2.inPool);

            if (inPoolCount <= minimumCapacity)
            {
                AddToPool(5);
                inPoolCount = _list.Count(x => x.Item2.inPool);
            }

            if (inPoolCount == 0)
            {
                Debug.LogError(errString + $"Empty. Consider setting a higher {nameof(minimumCapacity)}");
                return null;
            }

            var tuple = _list.FirstOrDefault(x => x.Item2.inPool);
            var transform = tuple.Item1.transform;
            transform.position = position;
            transform.rotation = rotation;
            if (parent) transform.parent = parent;
            tuple.Item2.AwakeFromPool();

            return tuple.Item1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="objectInstantiationAction"></param>
        /// <returns></returns>
        public static AssetPool<TComponent> GetOrCreate(string key, Action<TComponent> objectInstantiationAction)
        {
            return GetOrCreate(key, 10, null, objectInstantiationAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="initialCapacity"></param>
        /// <param name="objectsParent"></param>
        /// <param name="objectInstantiationAction"></param>
        /// <returns></returns>
        public static AssetPool<TComponent> GetOrCreate(string key, int initialCapacity = 10, Transform objectsParent = null, Action<TComponent> objectInstantiationAction = null)
        {
            if (TryGetPool(key, out var existingPool))
            {
                return existingPool;
            }
            var pool = new AssetPool<TComponent>(key, initialCapacity, objectsParent, objectInstantiationAction);
            return pool;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aRef"></param>
        /// <param name="assetReferencePool"></param>
        /// <returns></returns>
        public static bool TryGetPool(string key, out AssetPool<TComponent> assetPool)
        {
            if (AllPools.TryGetValue(key, out var p))
            {
                assetPool = p as AssetPool<TComponent>;
                return true;
            }

            assetPool = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        public void TryGetPoolObject(Vector3 position, Quaternion rotation, Transform parent, out AsyncOperationHandle<TComponent> handle)
        {
            TryTake(position, rotation, parent, out handle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemovePool(string key)
        {
            if (AllPools.ContainsKey(key))
            {
                AssetManager.Unload(key);
                AllPools.Remove(key);
                return true;
            }

            return false;
        }
    }
}
