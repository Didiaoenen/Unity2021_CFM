local INotifiable = class("INotifiable") as INotifiableType

function INotifiable.opt:ValueChanged(value: OptHandlerType)
    assert({value}, "")
end

return INotifiable