

require("OOP.Class")

local InteractionEventArgs = class("InteractionEventArgs") as InteractionEventArgsType

function InteractionEventArgs.get:Context(): ObjectType
    return self.context
end

function InteractionEventArgs.get:Callback(): function
    return self.callback
end

function InteractionEventArgs:ctor(context: ObjectType, callback: function)
    self.context = context
    self.callback = callback
end

function InteractionEventArgs:dtor()
end

return InteractionEventArgs