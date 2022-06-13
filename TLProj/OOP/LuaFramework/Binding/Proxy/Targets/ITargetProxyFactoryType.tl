local Object = require("OOP.ObjectType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")
local TargetProxyBase = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")

local record ITargetProxyFactory
    CreateProxy: function(ITargetProxyFactory, Object, BindingDescription): TargetProxyBase
end

return ITargetProxyFactory