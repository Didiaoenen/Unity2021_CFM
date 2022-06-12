local Object = require("OOP.ObjectType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")

local record INodeProxyFactory
    Create: function(INodeProxyFactory, Object, {string}): SourceProxyBase
end
    
return INodeProxyFactory