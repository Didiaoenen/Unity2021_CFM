local EventHandler = require("OOP.LuaFramework.EventHandlerType")

local record Set
    executeChanged: {EventHandler}
    ExecuteChanged: function
end

local record CommandBase
    set: Set

    executeChanged: {EventHandler}
    ExecuteChanged: EventHandler

    ctor: function
    dtor: function
    RaiseExecuteChanged: function(CommandBase)
    CanExecute: function(CommandBase, ...: any)
    Execute: function(CommandBase, ...: any)
end

return CommandBase