using System.Collections.Generic;
using System;
namespace CFM.Framework.Services
{
    public class ServiceContainer : IServiceContainer, IDisposable
    {
        private Dictionary<string, IFactory> services = new Dictionary<string, IFactory>();

        public virtual object Resolve(Type type)
        {
            return Resolve<object>(type.Name);
        }

        public virtual T Resolve<T>()
        {
            return Resolve<T>(typeof(T).Name);
        }

        public virtual object Resolve(string name)
        {
            return Resolve<object>(name);
        }

        public virtual T Resolve<T>(string name)
        {
            IFactory factory;
            if (services.TryGetValue(name, out factory))
                return (T)factory.Create();
            return default(T);
        }

        public virtual void Register<T>(Func<T> factory)
        {
            Register<T>(typeof(T).Name, factory);
        }

        public virtual void Register(Type type, object target)
        {
            Register<object>(type.Name, target);
        }

        public virtual void Register(string name, object target)
        {
            Register<object>(name, target);
        }

        public virtual void Register<T>(T target)
        {
            Register<T>(typeof(T).Name, target);
        }

        public virtual void Register<T>(string name, Func<T> factory)
        {
            if (services.ContainsKey(name))
                throw new DuplicateRegisterServiceException(string.Format("Duplicate key {0}", name));

            services.Add(name, new GenericFactory<T>(factory));
        }

        public virtual void Register<T>(string name, T target)
        {
            if (services.ContainsKey(name))
                throw new DuplicateRegisterServiceException(string.Format("Duplicate key {0}", name));

            services.Add(name, new SingleInstanceFactory(target));
        }

        public virtual void Unregister(Type type)
        {
            Unregister(type.Name);
        }

        public virtual void Unregister<T>()
        {
            Unregister(typeof(T).Name);
        }

        public virtual void Unregister(string name)
        {
            IFactory factory;
            if (services.TryGetValue(name, out factory))
                factory.Dispose();

            services.Remove(name);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    foreach (var kv in services)
                        kv.Value.Dispose();

                    services.Clear();
                }
                disposed = true;
            }
        }

        ~ServiceContainer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal interface IFactory : IDisposable
        {
            object Create();
        }

        internal class GenericFactory<T> : IFactory
        {
            private Func<T> func;

            public GenericFactory(Func<T> func)
            {
                this.func = func;
            }

            public virtual object Create()
            {
                return func();
            }

            public void Dispose()
            {

            }
        }

        internal class SingleInstanceFactory : IFactory
        {
            private object target;

            public SingleInstanceFactory(object target)
            {
                this.target = target;
            }

            public virtual object Create()
            {
                return target;
            }

            private bool disposed = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        var disposable = target as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                        target = null;
                    }
                    disposed = true;
                }
            }

            ~SingleInstanceFactory()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}

