local IInteractionRequest = require("LuaFramework.Interactivity.IInteractionRequest")
local InteractionEventArgs = require("LuaFramework.Interactivity.InteractionEventArgs")
local TableOprationMode = require("LuaFramework.Binding.TableOprationMode")

local InteractionRequest = class("InteractionRequest", IInteractionRequest) as InteractionRequestType

function InteractionRequest.opt:Raised(value: OptHandlerType)

    if value.opt == TableOprationMode.Add then
        self.raised[value.save] = value
    end
    
    if value.opt == TableOprationMode.Remove then
        self.raised[value.save] = nil
    end
end

function InteractionRequest.get:Raised(): {string: OptHandlerType}
    return self.raised
end

function InteractionRequest:ctor(sender: LuaObject)
    self.sender = sender or (self as LuaObject)
    self.raised = nil
end

function InteractionRequest:Raise(context: LuaObject, callback: function)
    if nil == self.Raised then
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