local INotifiable = class("INotifiable") as INotifiableType

function INotifiable.set:ValueChanged(value: EventHandlerType)
    assert({value}, "")
end

return INotifiable