using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public abstract class AssetReferencePool : IPool, IDisposable
{
    public const int MinimumCapacityDefault = 5;

    protected static readonly Dictionary<object, AssetReferencePool> AllPools = new Dictionary<object, AssetReferencePool>();

    protected abstract Object loadedObject { get; }

    protected abstract ICollection collection { get; }

    bool _disposed;

    protected bool disposed => _disposed;

    protected bool _isReady;

    public virtual bool isReady => _isReady;

    public readonly AssetReference assetReference;

    public readonly int initialCapacity;

    public readonly Transform objectsParent;

    public int minimumCapacity;

    protected AssetReferencePool(AssetReference assetReference, int initialCapacity, Transform objectsParent)
    {
        this.assetReference = assetReference;
        this.initialCapacity = initialCapacity;
        this.objectsParent = objectsParent;
        this.minimumCapacity = MinimumCapacityDefault;

        AllPools.Add(assetReference.RuntimeKey, this);
    }

    ~AssetReferencePool()
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
            AllPools.Remove(assetReference.RuntimeKey);
            _isReady = false;
        }

        _disposed = true;
    }

    public static bool TryGetPool(AssetReference aRef, out AssetReferencePool assetReferencePool)
    {
        return AllPools.TryGetValue(aRef.RuntimeKey, out assetReferencePool);
    }

    public static bool TryGetPool(object key, out AssetReferencePool assetReferencePool)
    {
        return AllPools.TryGetValue(key, out assetReferencePool);
    }

    protected abstract void AddToPool(int count);

    public abstract void PoolObjectReturned(PoolObject poolObject);

    public static void LogAllPools()
    {
        foreach (var kvp in AllPools)
        {
            Debug.Log($"{nameof(AssetReferencePool)}: Key {kvp.Key} | Value {kvp.Value} - {kvp.Value.collection.Count} instantiated objects.");
        }
    }
}

public class AssetReferencePool<TComponent> : AssetReferencePool where TComponent : Component
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

    protected AssetReferencePool(AssetReference assetReference, int initialCapacity, Transform objectsParent, Action<TComponent> objectInstantiatedAction)
        : base(assetReference, initialCapacity, objectsParent)
    {
        _list = new List<Tuple<TComponent, PoolObject>>(initialCapacity);
        _objectInstantiatedAction = objectInstantiatedAction;

        AssetManager.OnAssetLoaded += OnObjectLoaded;
        AssetManager.OnAssetUnloaded += OnObjectUnloaded;

        if (AssetManager.TryGetOrLoadComponentAsync(assetReference, out _loadHandle))
        {
            OnObjectLoaded(_loadHandle.Result);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

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
        OnObjectLoaded(null);
    }

    void OnObjectLoaded(TComponent obj)
    {
        //_loadedObject = obj;

        _isReady = true;

        AddToPoolSyncSafe(initialCapacity);

        OnPoolReady?.Invoke();
    }

    void OnObjectUnloaded(object runtimeKey)
    {
        if (runtimeKey.ToString() != assetReference.RuntimeKey.ToString())
        {
            return;
        }

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

        AssetManager.TryInstantiateMultiSync<TComponent>(assetReference, count, pos, Quaternion.identity, objectsParent, out var instanceList);
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
    /// <param name="aRef"></param>
    /// <param name="objectInstantiationAction"></param>
    /// <returns></returns>
    public static AssetReferencePool<TComponent> GetOrCreate(AssetReference aRef, Action<TComponent> objectInstantiationAction)
    {
        return GetOrCreate(aRef, 10, null, objectInstantiationAction);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aRef"></param>
    /// <param name="initialCapacity"></param>
    /// <param name="objectsParent"></param>
    /// <param name="objectInstantiationAction"></param>
    /// <returns></returns>
    public static AssetReferencePool<TComponent> GetOrCreate(AssetReference aRef, int initialCapacity = 10, Transform objectsParent = null, Action<TComponent> objectInstantiationAction = null)
    {
        if (TryGetPool(aRef, out var existingPool))
        {
            return existingPool;
        }
        var pool = new AssetReferencePool<TComponent>(aRef, initialCapacity, objectsParent, objectInstantiationAction);
        return pool;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aRef"></param>
    /// <param name="assetReferencePool"></param>
    /// <returns></returns>
    public static bool TryGetPool(AssetReference aRef, out AssetReferencePool<TComponent> assetReferencePool)
    {
        if (AllPools.TryGetValue(aRef.RuntimeKey, out var p))
        {
            assetReferencePool = p as AssetReferencePool<TComponent>;
            return true;
        }

        assetReferencePool = null;
        return false;
    }

    public void TryGetPoolObject(Vector3 position, Quaternion rotation, Transform parent, out AsyncOperationHandle<TComponent> handle)
    {
        TryTake(position, rotation, parent, out handle);
    }
}
