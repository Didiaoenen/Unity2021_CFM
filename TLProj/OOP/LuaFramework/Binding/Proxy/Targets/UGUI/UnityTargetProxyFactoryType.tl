local Object = require("OOP.ObjectType")
local TargetProxyBase = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")

local record UnityTargetProxyFactory
    new: function
    ctor: function
    dtor: function
    CreateProxy: function(UnityTargetProxyFactory, Object, BindingDescription): TargetProxyBase
end

return UnityTargetProxyFactory