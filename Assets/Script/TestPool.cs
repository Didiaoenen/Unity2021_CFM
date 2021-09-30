using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestPool : MonoBehaviour
{
    public AssetReference assetReference;

    AssetReferencePool<BoxCollider> b;

    private void Awake()
    {
        b = AssetReferencePool<BoxCollider>.GetOrCreate(assetReference, callback =>
        {
            callback.GetComponent<BoxCollider>();
        });
    }

    public void onClickBtn()
    {
        b.TryGetPoolObject(Vector3.zero, Quaternion.identity, null, out var handle);
    }

    public void onClickDel()
    {
        AssetManager.Unload(assetReference);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
