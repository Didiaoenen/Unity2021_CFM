using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Mvvm;
public class TestPool : MonoBehaviour
{
    string prefabKey = "Assets/Prefabs/Cube.prefab";

    AssetPool<BoxCollider> b;

    private void Awake()
    {

    }

    private void Start()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(onCreatePool);
        transform.Find("Button (1)").GetComponent<Button>().onClick.AddListener(onClickBtn);
        transform.Find("Button (2)").GetComponent<Button>().onClick.AddListener(onClickDel);
    }

    public void onCreatePool()
    {
        b = AssetPool<BoxCollider>.GetOrCreate(prefabKey, callback =>
        {
            callback.GetComponent<BoxCollider>();
        });
    }

    public void onClickBtn()
    {
        //b.TryTake(Vector3.zero, Quaternion.identity, null, out var handle);
        b.TryGetPoolObject(Vector3.zero, Quaternion.identity, null, out var handle);
    }

    public void onClickDel()
    {
        AssetPool<BoxCollider>.RemovePool(prefabKey);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
