

require("OOP.Class")

local SourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.SourceProxyBase")
local IModifiable = require("LuaFramework.Binding.Proxy.IModifiable")

local InteractionNodeProxy = class("InteractionNodeProxy", SourceProxyBase, IModifiable) as InteractionNodeProxyType

function InteractionNodeProxy:ctor(source: ObjectType, request: string)
    SourceProxyBase.ctor(self, source)
    self.request = request
    self.invoker = nil
    self:BindEvent()
end

function InteractionNodeProxy:BindEvent()
    local anyType = Functions.ClassGet(self.source, self.request) as InteractionRequestType
    if anyType then
        anyType.Raised = function(sender: ObjectType, ...: any)
            self:OnRaised(sender, ...)
        end
    end
end

function InteractionNodeProxy:UnbindEvent()
    local anyType = Functions.ClassGet(self.source, self.request) as InteractionRequestType
    if anyType then
        anyType.Raised = nil
    end
end

function InteractionNodeProxy:OnRaised(sender: ObjectType, ...: any)
    if self.invoker then
        local invokerType = self.invoker as ProxyInvokerType
        invokerType:Invoke(sender, ...)
    end
end

function InteractionNodeProxy:SetValue(value: ObjectType)
    self.invoker = value
end

function InteractionNodeProxy:dtor()
end

return InteractionNodeProxy