local Object = require("OOP.ObjectType")
local BindingContext = require("OOP.LuaFramework.Binding.Contexts.BindingContextType")
local BindingBuilderBase = require("OOP.LuaFramework.Binding.Builder.BindingBuilderBaseType")

local record BindingSet
    builders: {BindingBuilderBase}
    context: BindingContext
    target: Object

    new: function(...: any): BindingSet
    ctor: function
    dtor: function
    Bind: function(BindingSet): BindingBuilderBase
    BindByTarget: function(BindingSet, Object): BindingBuilderBase
end

return BindingSet