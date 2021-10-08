using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class AddSprite : MonoBehaviour
{
    GameObject uiPrefab;

    string label = "preload";
    string prefabAddress = "Assets/Prefabs/Image.prefab";
    string spriteAddress = "Assets/Texture/icon_facebook.png";

    private void Start()
    {
        transform.Find("Button (3)").GetComponent<Button>().onClick.AddListener(addPrefab);
        transform.Find("Button (4)").GetComponent<Button>().onClick.AddListener(addSprite);
        transform.Find("Button (5)").GetComponent<Button>().onClick.AddListener(clickDel);
    }

    public void addPrefab()
    {
        Debug.Log(Time.realtimeSinceStartup);

        //var handle = Addressables.LoadAssetsAsync<Sprite>((IEnumerable)new List<object> { "preload" }, null, Addressables.MergeMode.Intersection);
        //await handle.Task;

        AssetManager.TryGetOrLoadObjectAsync<GameObject>(prefabAddress, out var handle);
        AssetManager.OnAssetLoaded += OnPrefabLoaded;

        Debug.Log(Time.realtimeSinceStartup);
    }

    public void addSprite()
    {
        AssetManager.TryGetOrLoadObjectAsync<Sprite>(spriteAddress, out var handle);
        AssetManager.OnAssetLoaded += OnSpriteLoaded;
    }

    void clickDel()
    {
        //AssetManager.UnloadByLabel(spriteAddress);
        //AssetManager.Unload(spriteAddress);
        AssetManager.Unload(prefabAddress);
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    void OnSpriteLoaded(object key, AsyncOperationHandle handle)
    {
        uiPrefab.GetComponent<MonoTracker>().AddKey((string)key);
        uiPrefab.GetComponent<Image>().sprite = handle.Convert<Sprite>().Result;
        AssetManager.OnAssetLoaded -= OnSpriteLoaded;
    }

    void OnPrefabLoaded(object key, AsyncOperationHandle handle)
    {
        AssetManager.TryInstantiateSync((string)key, Vector3.zero, Quaternion.identity, transform, out uiPrefab);
        uiPrefab.transform.localPosition = Vector3.zero;
        uiPrefab.transform.localRotation = Quaternion.identity;
        AssetManager.OnAssetLoaded -= OnPrefabLoaded;
    }
}
