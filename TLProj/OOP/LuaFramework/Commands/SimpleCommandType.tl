local EventHandler = require("OOP.LuaFramework.EventHandlerType")

local record Get
    enabled: boolean
    Enabled: function
end

local record Set
    enabled: boolean
    Enabled: function

    executeChanged: {EventHandler}
    ExecuteChanged: function

    RaiseCanExecuteChanged: function(Set)
end

local record SimpleCommand
    get: Get
    set: Set

    enabled: boolean
    Enabled: boolean

    executeChanged: {EventHandler}
    ExecuteChanged: EventHandler

    execute: function

    new: function(...: any): SimpleCommand
    ctor: function
    dtor: function
    RaiseExecuteChanged: function(SimpleCommand)
    CanExecute: function(SimpleCommand, ...: any)
    Execute: function(SimpleCommand, ...: any)
end

return SimpleCommand