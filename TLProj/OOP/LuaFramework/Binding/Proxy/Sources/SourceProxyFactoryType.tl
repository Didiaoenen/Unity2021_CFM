local Object = require("OOP.ObjectType")
local OutProxy = require("OOP.LuaFramework.Binding.Proxy.OutProxyType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
local SourceDescription = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")
local ISourceProxyFactory = require("OOP.LuaFramework.Binding.Proxy.Sources.ISourceProxyFactoryType")
local PriorityFactoryPair = require("OOP.LuaFramework.Binding.Proxy.PriorityFactoryPairType")

local type PriorityFactoryPairList = {PriorityFactoryPair}

local record SourceProxyFactory
    factories: PriorityFactoryPairList

    new: function
    ctor: function
    dtor: function
    CreateProxy: function(SourceProxyFactory, Object, SourceDescription): SourceProxyBase
    TryCreateProxy: function(SourceProxyFactory, Object, SourceDescription, OutProxy): boolean
    Register: function(SourceProxyFactory, PriorityFactoryPair, number)
    Unregister: function(SourceProxyFactory, ISourceProxyFactory)
end

return SourceProxyFactory