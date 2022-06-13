local Object = require("OOP.ObjectType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
local TargetProxyBase = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")
local Binding = require("OOP.LuaFramework.Binding.BindingType")

local record StandardBinder
    
    sourceProxyFactory: SourceProxyBase
    targetProxyFactory: TargetProxyBase

    __singleton: function(StandardBinder): StandardBinder

    new: function(...: any): StandardBinder
    ctor: function
    delete: function
    dtor: function

    Bind: function(StandardBinder, Object, Object, BindingDescription): Binding
end

return StandardBinder