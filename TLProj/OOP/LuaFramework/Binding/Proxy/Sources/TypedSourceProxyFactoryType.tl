local Object = require("OOP.ObjectType")
local OutProxy = require("OOP.LuaFramework.Binding.Proxy.OutProxyType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
local SourceDescription = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")

local record SourceProxyFactory
    ctor: function
    dtor: function
    IsSupported: function(SourceProxyFactory, SourceDescription): boolean
    CreateProxy: function(SourceProxyFactory, Object, SourceDescription): SourceProxyBase
    TryCreateProxy: function(SourceProxyFactory, Object, SourceDescription, OutProxy): boolean
end

return SourceProxyFactory