

require("OOP.Class")

local INotifiable = class("INotifiable") as INotifiableType

function INotifiable.set:ValueChanged(value: EventHandlerType)
    print(value)
end

return INotifiable