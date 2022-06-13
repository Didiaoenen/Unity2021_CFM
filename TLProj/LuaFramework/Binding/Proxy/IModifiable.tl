

require("OOP.Class")

local IModifiable = class("IModifiable") as IModifiableType

function IModifiable:SetValue(value: any)
    print(value)
end

return IModifiable