local Object = require("OOP.ObjectType")
local BindingSet = require("OOP.LuaFramework.Binding.Builder.BindingSetType")
local BindingContext = require("OOP.LuaFramework.Binding.Contexts.BindingContextType")

local record Get
    bindingContext: BindingContext
    BindingContext: function

    behaviour: Object
end

local record BindingExtension
    get: Get

    bindingContext: BindingContext
    BindingContext: BindingContext

    behaviour: Object

    new: function(...: any): BindingExtension
    ctor: function
    dtor: function

    CreateBindingSet: function(BindingExtension): BindingSet
    CreateBindingSetWithContext: function(BindingExtension, Object): BindingSet
    SetDataContext: function(BindingExtension, Object)
    GetDataContext: function(BindingExtension): Object
end

return BindingExtension