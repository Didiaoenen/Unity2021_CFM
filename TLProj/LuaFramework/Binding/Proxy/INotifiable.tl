

require("OOP.Class")

local INotifiable = class("INotifiable") as INotifiableType

function INotifiable.get:ValueChanged()
end

return INotifiable