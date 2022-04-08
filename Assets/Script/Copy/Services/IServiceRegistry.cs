using System;
namespace CFM.Framework.Services
{
    public interface IServiceRegistry
    {
        void Register(Type type, object target);

        void Register<T>(T target);

        void Register(string name, object target);

        void Register<T>(string name, T target);

        void Register<T>(Func<T> factory);

        void Register<T>(string name, Func<T> factory);

        void UnRegister<T>();

        void UnRegister(Type type);

        void UnRegister(string name);
    }
}

