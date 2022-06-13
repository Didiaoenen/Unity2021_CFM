local Object = require("OOP.ObjectType")
local PropertyChangedEventArgs = require("OOP.LuaFramework.Binding.PropertyChangedEventArgsType")

local record PropertyNodeProxy
    source: Object
    propertyInfo: string
    
    new: function(...: any): PropertyNodeProxy
    ctor: function
    dtor: function

    OnPropertyChanged: function(PropertyNodeProxy, Object, PropertyChangedEventArgs)
    RaiseValueChanged: function(PropertyNodeProxy)
    GetValue: function(PropertyNodeProxy): any
    SetValue: function(PropertyNodeProxy, any)
end

return PropertyNodeProxy