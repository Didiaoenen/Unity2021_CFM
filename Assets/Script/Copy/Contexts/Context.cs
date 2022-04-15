using System;
using System.Collections;
using System.Collections.Generic;

using CFM.Framework.Services;

namespace CFM.Framework.Contexts
{
    public class Context : IDisposable
    {
        private bool innerContainer = false;

        private Context contextBase;

        private IServiceContainer container;

        private Dictionary<string, object> attributes;

        public Context() : this(null, null)
        {

        }

        public Context(IServiceContainer container, Context contextBase)
        {
            attributes = new Dictionary<string, object>();
            this.container = container;
            this.contextBase = contextBase;
            if (this.container == null)
            {
                innerContainer = true;
                this.container = new ServiceContainer();
            }
        }

        public virtual bool Contains(string name, bool cascade = true)
        {
            if (attributes.ContainsKey(name))
                return true;

            if (cascade && contextBase != null)
                return contextBase.Contains(name, cascade);

            return false;
        }

        public virtual object Get(string name, bool cascade = true)
        {
            return Get<object>(name, cascade);
        }

        public virtual T Get<T>(string name, bool cascade = true)
        {
            object v;
            if (attributes.TryGetValue(name, out v))
                return (T)v;

            if (cascade && contextBase != null)
                return contextBase.Get<T>(name, cascade);

            return default(T);
        }

        public virtual void Set(string name, object value)
        {
            Set<object>(name, value);
        }

        public virtual void Set<T>(string name, T value)
        {
            attributes[name] = value;
        }

        public virtual object Remove(string name)
        {
            return Remove<object>(name);
        }

        public virtual T Remove<T>(string name)
        {
            if (!attributes.ContainsKey(name))
                return default(T);

            object v = attributes[name];
            attributes.Remove(name);
            return (T)v;
        }

        public virtual IEnumerator GetEnumerator()
        {
            return attributes.GetEnumerator();
        }

        public virtual IServiceContainer GetContainer()
        {
            return container;
        }

        public virtual object GetService(Type type)
        {
            object result = container.Resolve(type);
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService(type);

            return null;
        }

        public virtual object GetService(string name)
        {
            object result = container.Resolve(name);
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService(name);

            return null;
        }

        public virtual T GetService<T>()
        {
            T result = container.Resolve<T>();
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService<T>();

            return default(T);
        }

        public virtual T GetService<T>(string name)
        {
            T result = container.Resolve<T>(name);
            if (result != null)
                return result;

            if (contextBase != null)
                return contextBase.GetService<T>(name);

            return default(T);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (innerContainer && container != null)
                    {
                        IDisposable dis = container as IDisposable;
                        if (dis != null)
                            dis.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~Context()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class ContextManager
    {
        private static Dictionary<string, Context> contexts = new Dictionary<string, Context>();

        private static Context context = new ApplicationContext();

        public static ApplicationContext GetApplicationContext()
        {
            return (ApplicationContext)context;
        }

        public static void SetApplicationContext(ApplicationContext context)
        {
            ContextManager.context = context;
        }

        public static Context GetContext(string key)
        {
            Context context = null;
            contexts.TryGetValue(key, out context);
            return null;
        }

        public static T GetContext<T>(string key) where T : Context
        {
            return (T)GetContext(key);
        }

        public static void AddContext(string key, Context context)
        {
            contexts.Add(key, context);
        }

        public static void RemoveContext(string key)
        {
            contexts.Remove(key);
        }
    }
}

