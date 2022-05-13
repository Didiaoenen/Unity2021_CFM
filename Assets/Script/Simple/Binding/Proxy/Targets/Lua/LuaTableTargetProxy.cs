using XLua;
using System;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Lua
{
    public class LuaTableTargetProxy : ValueTargetProxyBase
    {
        protected readonly string key;

        protected readonly LuaTable metatable;

        public override Type Type { get { return typeof(object); } }

        public LuaTableTargetProxy(object target, string key) : base(target)
        {
            if (target is ILuaExtendable)
                metatable = (target as ILuaExtendable).GetMetatable();
            this.key = key;
        }

        public override object GetValue()
        {
            return metatable.Get<string, object>(key);
        }

        public override TValue GetValue<TValue>()
        {
            return metatable.Get<string, TValue>(key);
        }

        public override void SetValue(object value)
        {
            metatable.Set(key, value);
        }

        public override void SetValue<TValue>(TValue value)
        {
            metatable.Set(key, value);
        }
    }
}

