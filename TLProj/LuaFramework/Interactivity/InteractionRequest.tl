

require("OOP.Class")

local InteractionEventArgs = require("LuaFramework.Interactivity.InteractionEventArgs")

local InteractionRequest = class("InteractionRequest") as InteractionRequestType

function InteractionRequest.get:Raised(): function
    return self.raised
end

function InteractionRequest.set:Raised(value: function)
    self.raised = value
end

function InteractionRequest:ctor(sender: ObjectType)
    self.sender = sender or (self as ObjectType)
    self.raised = nil
end

function InteractionRequest:Raise(context: ObjectType, callback: function)
    if nil == self.raised then
        return
    end
    self.Raised(self.sender, InteractionEventArgs.new(context, function()
        if callback then
            callback(context)
        end
    end))
end

function InteractionRequest:dtor()
end

return InteractionRequest