using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CFM.Framework.Execution;
using CFM.Framework.Asynchronous;

namespace CFM.Framework.Views.Locators
{
    public class DefaultUIViewLocator : UIViewLocatorBase
    {
        private GlobalWindowManager globalWindowManager;

        private Dictionary<string, WeakReference> templates = new Dictionary<string, WeakReference>();

        protected string Normalize(string name)
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

            globalWindowManager = GameObject.FindObjectOfType<GlobalWindowManager>();
            if (globalWindowManager == null)
                throw new NotFoundException("GlobalWindowManager");

            return globalWindowManager;
        }

        public override T LoadView<T>(string name)
        {
            return DoLoadView<T>(name);
        }

        public virtual T DoLoadView<T>(string name)
        {
            name = Normalize(name);
            WeakReference weakRef;
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

            GameObject go = GameObject.Instantiate(viewTemplateGo);
            go.name = viewTemplateGo.name;
            T view = go.GetComponent<T>();
            if (view == null && go != null)
                GameObject.Destroy(go);
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
            name = Normalize(name);
            WeakReference wekRef;
            GameObject viewTemplateGo = null;
            try
            {
                if (templates.TryGetValue(name, out wekRef) && wekRef.IsAlive)
                {
                    viewTemplateGo = (GameObject)wekRef.Target;

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

            GameObject go = GameObject.Instantiate(viewTemplateGo);
            go.name = viewTemplateGo.name;
            T view = go.GetComponent<T>();
            if (view == null)
            {
                GameObject.Destroy(go);
                promise.SetException(new NotFoundException(name));
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
                windowManager= GetDefaultWindowManager();

            ProgressResult<float, T> result = new ProgressResult<float, T>();
            Executors.RunOnCoroutineNoReturn(DoLoad<T>(result, name, windowManager));
            return result;
        }
    }
}
