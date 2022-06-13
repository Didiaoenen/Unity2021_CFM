local Object = require("OOP.ObjectType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")

local record UniversalNodeProxyFactory
    new: function(...: any): UniversalNodeProxyFactory
    ctor: function
    dtor: function
    Create: function(UniversalNodeProxyFactory, Object, {string}): SourceProxyBase
    CreateProxy: function(UniversalNodeProxyFactory, Object, string): SourceProxyBase
end

return UniversalNodeProxyFactory