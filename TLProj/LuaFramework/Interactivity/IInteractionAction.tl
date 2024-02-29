local IInteractionAction = class("IInteractionAction") as IInteractionActionType

function IInteractionAction:OnRequest(sender: LuaObject, args: InteractionEventArgsType)
    assert({sender, args})
end

return IInteractionAction