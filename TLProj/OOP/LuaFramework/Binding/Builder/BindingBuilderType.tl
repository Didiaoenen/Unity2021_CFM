local Object = require("OOP.ObjectType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")

local record BindingBuilder
    target: Object
    description: BindingDescription

    new: function(...: any): BindingBuilder
    ctor: function
    dtor: function
    For: function(BindingBuilder, string, string): BindingBuilder
    To: function(BindingBuilder, string): BindingBuilder
    OneWay: function(BindingBuilder): BindingBuilder
    OneWayToSource: function(BindingBuilder): BindingBuilder
    TwoWay: function(BindingBuilder): BindingBuilder
    OneTime: function(BindingBuilder): BindingBuilder
    CommandParameter: function(BindingBuilder, ...: any): BindingBuilder
    SetMode: function(BindingBuilder, number)
    SetMemberPath: function(BindingBuilder, string)
    SetCommandParameter: function(BindingBuilder, ...: any)
end

return BindingBuilder