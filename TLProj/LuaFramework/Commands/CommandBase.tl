

require("OOP.Class")

local CommandBase = class("CommandBase") as CommandBaseType

function CommandBase.set:ExecuteChanged(value: EventHandlerType)
    setcallback(self.executeChanged, value)
end

function CommandBase:ctor()
    self.executeChanged = {}
end

function CommandBase:RaiseExecuteChanged()
    for _, v in ipairs(self.executeChanged) do
        v.callback(v.this)
    end
end

function CommandBase:CanExecute(...: any)
    print(...)
end

function CommandBase:Execute(...: any)
    print(...)
end

function CommandBase:dtor()
end

return CommandBase