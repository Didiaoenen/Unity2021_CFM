using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

using CFM.Framework.Asynchronous;
using CFM.Framework.Views;
using CFM.Framework.Execution;

public class ResourcesViewLocator : UIViewLocatorBase
{
    private GlobalWindowManager globalWindowManager;

    private Dictionary<string, WeakReference> templates = new Dictionary<string, WeakReference>();

    protected string Normaliza(string name)
    {
        int index = name.IndexOf('.');
        if (index < 0)
            return name;

        return name.Substring(0, index);
    }

    protected virtual IWindowManager GetDefaultWindowManager()
    {
        if (globalWindowManager != null)
            return globalWindowManager;

        globalWindowManager = Object.FindObjectOfType<GlobalWindowManager>();
        if (globalWindowManager == null)
            throw new NotFoundException();

        return globalWindowManager;
    }

    public override T LoadView<T>(string name)
    {
        return DoLoadView<T>(name);
    }

    protected virtual T DoLoadView<T>(string name)
    {
        name = Normaliza(name);
        WeakReference weakRef = null;
        GameObject viewTemplateGo = null;
        try
        {
            if (templates.TryGetValue(name, out weakRef) && weakRef.IsAlive)
            {
                viewTemplateGo = (GameObject)weakRef.Target;

                if (viewTemplateGo != null)
                {
                    string goName = viewTemplateGo.name;
                }
            }
        }
        catch (Exception)
        {
            viewTemplateGo = null;
        }

        if (viewTemplateGo == null)
        {
            viewTemplateGo = Resources.Load<GameObject>(name);
            if (viewTemplateGo != null)
            {
                viewTemplateGo.SetActive(false);
                templates[name] = new WeakReference(viewTemplateGo);
            }
        }

        if (viewTemplateGo == null || viewTemplateGo.GetComponent<T>() == null)
            return default(T);

        GameObject go = Object.Instantiate(viewTemplateGo);
        go.name = viewTemplateGo.name;
        T view = go.GetComponent<T>();
        if (view == null && go != null)
            Object.Destroy(go);
        return view;
    }

    public override IProgressResult<float, T> LoadViewAsync<T>(string name)
    {
        ProgressResult<float, T> result = new ProgressResult<float, T>();
        Executors.RunOnCoroutineNoReturn(DoLoad<T>(result, name));
        return result;
    }

    protected virtual IEnumerator DoLoad<T>(IProgressPromise<float, T> promise, string name, IWindowManager windowManager = null)
    {
        name = Normaliza(name);
        WeakReference weakRef = null;
        GameObject viewTemplateGo = null;
        try
        {
            if (templates.TryGetValue(name, out weakRef) && weakRef.IsAlive)
            {
                viewTemplateGo = (GameObject)weakRef.Target;

                if (viewTemplateGo != null)
                {
                    string goName = viewTemplateGo.name;
                }
            }
        }
        catch (Exception)
        {
            viewTemplateGo = null;
        }

        if (viewTemplateGo == null)
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(name);
            while (!request.isDone)
            {
                promise.UpdateProgress(request.progress);
                yield return null;
            }

            viewTemplateGo = (GameObject)request.asset;
            if (viewTemplateGo != null)
            {
                viewTemplateGo.SetActive(false);
                templates[name] = new WeakReference(viewTemplateGo);
            }
        }

        if (viewTemplateGo == null || viewTemplateGo.GetComponent<T>() == null)
        {
            promise.UpdateProgress(1f);
            promise.SetException(new NotFoundException(name));
            yield break;
        }

        GameObject go = Object.Instantiate(viewTemplateGo);
        go.name = viewTemplateGo.name;
        T view = go.GetComponent<T>();
        if (view == null)
        {
            Object.Destroy(go);
            promise.SetException(new DllNotFoundException(name));
        }
        else
        {
            if (windowManager != null && view is IWindow)
                (view as IWindow).WindowManager = windowManager;

            promise.UpdateProgress(1f);
            promise.SetResult(view);
        }
    }

    public override T LoadWindow<T>(string name)
    {
        return LoadWindow<T>(null, name);
    }

    public override T LoadWindow<T>(IWindowManager windowManager, string name)
    {
        if (windowManager == null)
            windowManager = GetDefaultWindowManager();

        T target = DoLoadView<T>(name);
        if (target != null)
            target.WindowManager = windowManager;

        return target;
    }

    public override IProgressResult<float, T> LoadWindowAsync<T>(string name)
    {
        return LoadWindowAsync<T>(null, name);
    }

    public override IProgressResult<float, T> LoadWindowAsync<T>(IWindowManager windowManager, string name)
    {
        if (windowManager == null)
            windowManager = GetDefaultWindowManager();

        ProgressResult<float, T> result = new ProgressResult<float, T>();
        Executors.RunOnCoroutineNoReturn(DoLoad(result, name, windowManager));
        return result;
    }
}
