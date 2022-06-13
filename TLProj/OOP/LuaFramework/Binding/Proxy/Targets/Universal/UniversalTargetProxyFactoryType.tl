local Object = require("OOP.ObjectType")
local TargetProxyBase = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")

local record UniversalTargetProxyFactory
    new: function(...: any): UniversalTargetProxyFactory
    ctor: function
    dtor: function
    CreateProxy: function(UniversalTargetProxyFactory, Object, BindingDescription): TargetProxyBase
end

return UniversalTargetProxyFactory