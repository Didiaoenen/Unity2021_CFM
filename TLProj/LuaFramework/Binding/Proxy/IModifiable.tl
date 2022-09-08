local IModifiable = class("IModifiable") as IModifiableType

function IModifiable:SetValue(value: any)
    assert({value}, "")
end

return IModifiable