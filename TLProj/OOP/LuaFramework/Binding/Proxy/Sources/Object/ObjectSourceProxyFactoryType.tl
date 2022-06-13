local Object = require("OOP.ObjectType")
local OutProxy = require("OOP.LuaFramework.Binding.Proxy.OutProxyType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
local SourceDescription = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")
local INodeProxyFactory = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.INodeProxyFactoryType")
local PriorityFactoryPair = require("OOP.LuaFramework.Binding.Proxy.PriorityFactoryPairType")

local record ObjectSourceProxyFactory
    factories: {PriorityFactoryPair}

    new: function(...: any): ObjectSourceProxyFactory
    ctor: function
    dtor: function
    Create: function(ObjectSourceProxyFactory, Object, {string}): SourceProxyBase
    TryCreateProxy: function(ObjectSourceProxyFactory, Object, SourceDescription, OutProxy): boolean
    Register: function(ObjectSourceProxyFactory, INodeProxyFactory, number)
    Unregister: function(ObjectSourceProxyFactory, INodeProxyFactory)
end

return ObjectSourceProxyFactory