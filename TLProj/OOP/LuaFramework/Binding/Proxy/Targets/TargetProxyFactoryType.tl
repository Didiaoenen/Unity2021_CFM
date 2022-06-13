local Object = require("OOP.ObjectType")
local OutProxy = require("OOP.LuaFramework.Binding.Proxy.OutProxyType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")
local TargetProxyBase = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")
local ITargetProxyFactory = require("OOP.LuaFramework.Binding.Proxy.Targets.ITargetProxyFactoryType")
local PriorityFactoryPair = require("OOP.LuaFramework.Binding.Proxy.PriorityFactoryPairType")

local type PriorityFactoryPairList = {PriorityFactoryPair}

local record TargetProxyFactory
    factories: PriorityFactoryPairList

    new: function(...: any): TargetProxyFactory
    ctor: function
    dtor: function
    CreateProxy: function(TargetProxyFactory, Object, BindingDescription): TargetProxyBase
    TryCreateProxy: function(TargetProxyFactory, Object, BindingDescription, OutProxy): boolean
    Register: function(TargetProxyFactory, ITargetProxyFactory, number)
    Unregister: function(TargetProxyFactory, ITargetProxyFactory)
end

return TargetProxyFactory