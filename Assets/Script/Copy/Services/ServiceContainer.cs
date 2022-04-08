using System.Collections.Generic;
using System;
namespace CFM.Framework.Services
{
    public class ServiceContainer : IServiceContainer, IDisposable
    {
        private Dictionary<string, IFactory> services = new Dictionary<string, IFactory>();

        public virtual object Resolve(Type type)
        {
            return null;
        }

        public virtual T Resolve<T>()
        {
            return default(T);
        }

        public virtual object Resolve(string name)
        {
            return this.Resolve<object>(name);
        }

        public virtual T Resolve<T>(string name)
        {
            IFactory factory;
            if (this.services.TryGetValue(name, out factory))
                return (T)factory.Create();
            return default(T);
        }

        public virtual void Register<T>(Func<T> factory)
        {

        }

        public virtual void Register(Type type, object target)
        {

        }

        public virtual void Register(string name, object target)
        {

        }

        public virtual void Register<T>(T target)
        {

        }

        public virtual void Register<T>(string name, Func<T> factory)
        {

        }

        public virtual void Register<T>(string name, T target)
        {

        }

        public virtual void UnRegister(Type type)
        {
            this.UnRegister(type.Name);
        }

        public virtual void UnRegister<T>()
        {

        }

        public virtual void UnRegister(string name)
        {

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

                    this.services.Clear();
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

            }

            public virtual object Create()
            {
                return null;
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
                return null;
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
                        this.target = null;
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

