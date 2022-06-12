local Object = require("OOP.ObjectType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
local SourceDescription = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")

local record ISourceProxyFactory
    CreateProxy: function(ISourceProxyFactory, Object, SourceDescription): SourceProxyBase
end

return ISourceProxyFactory