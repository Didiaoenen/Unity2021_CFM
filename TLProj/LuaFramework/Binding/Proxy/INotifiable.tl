local INotifiable = class("INotifiable") as INotifiableType

function INotifiable.set:ValueChanged(value: OptHandlerType)
    assert({value}, "")
end

return INotifiable