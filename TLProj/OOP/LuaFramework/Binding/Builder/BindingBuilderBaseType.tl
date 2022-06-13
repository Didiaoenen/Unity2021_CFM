local Object = require("OOP.ObjectType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")
local BindingContext = require("OOP.LuaFramework.Binding.Contexts.BindingContextType")

local record BindingBuilderBase
    context: BindingContext
    target: Object
    builded: boolean
    scopeKey: string
    description: BindingDescription

    ctor: function
    dtor: function
    SetMode: function(BindingBuilderBase, number)
    SetMemberPath: function(BindingBuilderBase, string)
    SetMemberPathWithPath: function(BindingBuilderBase, {string})
    SetCommandParameter: function(BindingBuilderBase, ...: any)
    CheckBindingDescription: function(BindingBuilderBase)
    Build: function(BindingBuilderBase)
end

return BindingBuilderBase