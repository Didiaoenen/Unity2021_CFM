local Object = require("OOP.ObjectType")
local Binding = require("OOP.LuaFramework.Binding.BindingType")
local EventHandler = require("OOP.LuaFramework.EventHandlerType")
local BindingDescription = require("OOP.LuaFramework.Binding.BindingDescriptionType")

local record Get
    dataContext: Object
    DataContext: function
end

local record Set
    dataContext: Object
    DataContext: function

    dataContextChanged: {EventHandler}
    DataContextChanged: function

    OnDataContextChanged: function
    RaiseDataContextChanged: function
end

local record BindingContext
    get: Get
    set: Set
    
    dataContext: Object
    DataContext: Object

    dataContextChanged: {EventHandler}
    DataContextChanged: EventHandler
    
    owner: Object
    bindings: {string:{Binding}}
    DEFAULT_KEY: string

    new: function(...: any): BindingContext
    ctor: function
    dtor: function
    OnDataContextChanged: function(BindingContext)
    RaiseDataContextChanged: function(BindingContext)
    GetOrCreateList: function(BindingContext, string): {Binding}
    Add: function(BindingContext, Binding, string)
    AddWithSingleDesc: function(BindingContext, Object, BindingDescription, string)
end

return BindingContext