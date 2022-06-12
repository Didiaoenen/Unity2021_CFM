local Object = require("OOP.ObjectType")
local PropertyChangedEventArgs = require("OOP.LuaFramework.Binding.Proxy.PropertyChangedEventArgsType")

local record Get
    DefaultMode: function
end

local record PropertyTargetProxy
    get: Get

    target: Object
    propertyInfo: string
    
    new: function
    ctor: function
    dtor: function

    DoSubscribeForValueChange: function(PropertyTargetProxy, Object)
    DoUnsubscribeForValueChange: function(PropertyTargetProxy, Object)
    OnPropertyChanged: function(PropertyTargetProxy, Object, PropertyChangedEventArgs)
    RaiseValueChanged: function(PropertyTargetProxy)
    GetValue: function(PropertyTargetProxy): any
    SetValue: function(PropertyTargetProxy, any)
end

return PropertyTargetProxy