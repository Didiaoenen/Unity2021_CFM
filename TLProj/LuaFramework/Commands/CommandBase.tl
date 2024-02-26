local ICommand = require("LuaFramework.Commands.ICommand")

local CommandBase = class("CommandBase", ICommand) as CommandBaseType

function CommandBase.opt:CanExecuteChanged(value: EventHandlerType)
    setcallback(self.canExecuteChanged, value)
end

function CommandBase:ctor()
    self.canExecuteChanged = {}
end

function CommandBase:RaiseExecuteChanged()
    for _, v in ipairs(self.canExecuteChanged) do
        v.callback(v.this)
    end
end

function CommandBase:CanExecute(...: any)
    assert({...}, "")
end

function CommandBase:Execute(...: any)
    assert({...}, "")
end

function CommandBase:dtor()
end

return CommandBase