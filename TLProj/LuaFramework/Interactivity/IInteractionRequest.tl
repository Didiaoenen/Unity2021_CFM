local IInteractionRequest = class("IInteractionRequest") as IInteractionRequestType

function IInteractionRequest.get:Raised(): function
    return nil
end

function IInteractionRequest.set:Raised(value: function)
    assert({value}, "")
end

return IInteractionRequest