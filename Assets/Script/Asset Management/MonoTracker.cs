using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTracker : MonoBehaviour
{
    public delegate void DelegateDestroyed(MonoTracker tracker);
    public event DelegateDestroyed OnDestroyed;

    private List<string> keyList = new List<string>();

    public object key { get; set; }

    public void AddKey(string key)
    {
        keyList.Add(key);
    }

    private void OnDestroy()
    {
        for (int i = keyList.Count - 1; i >= 0; i--)
        {
            AssetManager.Unload(keyList[i]);
        }
        OnDestroyed?.Invoke(this);
    }
}
