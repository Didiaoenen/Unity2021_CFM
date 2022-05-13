using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public interface ILuaObservableObject
    {
        void subscribe(string key, Action action);

        void unsubscribe(string key, Action action);

        void subscribe(int key, Action action);

        void unsubscribe(int key, Action action);
    }
}

