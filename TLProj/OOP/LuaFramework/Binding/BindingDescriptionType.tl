local Object = require("OOP.ObjectType")
local SourceDescription = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")

local record Get
    targetName: string
    TargetName: function

    updateTrigger: string
    UpdateTrigger: function

    mode: number
    Mode: function

    source: SourceDescription
    Source: function

    commandParameter: Object
    CommandParameter: function
end

local record Set
    targetName: string
    TargetName: function

    updateTrigger: string
    UpdateTrigger: function

    mode: number
    Mode: function

    source: SourceDescription
    Source: function

    commandParameter: Object
    CommandParameter: function
end

local record BindingDescription
    get: Get
    set: Set

    targetName: string
    TargetName: string

    updateTrigger: string
    UpdateTrigger: string

    mode: number
    Mode: number

    source: SourceDescription
    Source: SourceDescription

    commandParameter: Object
    CommandParameter: function

    new: function
    ctor: function
    dtor: function
end

return BindingDescription