local TargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.TargetProxyBase")
local IObtainable = require("LuaFramework.Binding.Proxy.IObtainable")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local InteractionTargetProxy = class("InteractionTargetProxy", TargetProxyBase, IObtainable) as InteractionTargetProxyType

function InteractionTargetProxy.get:DefaultMode(): number
    return BindingMode.OneWayToSource
end

function InteractionTargetProxy:ctor(target: LuaObject, interactionAction: IInteractionActionType)
    TargetProxyBase.ctor(self, target)
    self.interactionAction = interactionAction
end

function InteractionTargetProxy:GetValue(): any
    return self.handler
end

function InteractionTargetProxy:OnRequest(sender: LuaObject, args: InteractionEventArgsType)
    local target = self.Target
    if nil == target then
        return
    end

    self.interactionAction:OnRequest(sender, args)
end

return InteractionTargetProxy