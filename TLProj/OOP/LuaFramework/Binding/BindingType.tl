local Object = require("OOP.ObjectType")
local SourceProxyBase = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
local TargetProxyBase = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")
local ISourceProxyFactory = require("OOP.LuaFramework.Binding.Proxy.Sources.ISourceProxyFactoryType")
local ITargetProxyFactory = require("OOP.LuaFramework.Binding.Proxy.Targets.ITargetProxyFactoryType")
local SourceDescription = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")

local record Get
    bindingMode: number
    BindingMode: function

    targetProxy: TargetProxyBase
    bindingDescription: BindingDescription
end

local record Binding
    get: Get
    
    bindingMode: number
    BindingMode: number

    dataContext: Object
    DataContext: Object

    target: Object
    Target: Object

    targetProxy: TargetProxyBase
    sourceProxy: SourceProxyBase
    bindingDescription: BindingDescription
    sourceProxyFactory: ISourceProxyFactory
    targetProxyFactory: ITargetProxyFactory

    new: function(...: any): Binding
    ctor: function
    dtor: function

    CreateTargetProxy: function(Binding, Object, BindingDescription)
    IsSubscribeTargetValueChanged: function(Binding, number): boolean
    DisposeTargetProxy: function(Binding)
    CreateSourceProxy: function(Binding, Object, SourceDescription)
    IsSubscribeSourceValueChanged: function(Binding, number): boolean
    DisposeSourceProxy: function(Binding)
    UpdateTargetFromSource: function(Binding)
    SetTargetValue: function(Binding, Object, any)
    UpdateSourceFromTarget: function(Binding)
    SetSourceValue: function(Binding, Object, any)
    UpdateDataOnBind: function(Binding)
    UpdateTargetOnFirstBind: function(Binding, number)
    UpdateSourceOnFirstBind: function(Binding, number)
end

return Binding