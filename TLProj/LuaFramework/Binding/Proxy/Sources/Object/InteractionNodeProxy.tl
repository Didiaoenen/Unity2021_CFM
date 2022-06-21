

require("OOP.Class")

local SourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.SourceProxyBase")
local IModifiable = require("LuaFramework.Binding.Proxy.IModifiable")

local InteractionNodeProxy = class("InteractionNodeProxy", SourceProxyBase, IModifiable) as InteractionNodeProxyType

function InteractionNodeProxy:ctor(source: BaseType, request: string)
    SourceProxyBase.ctor(self, source)
    self.request = request
    self.invoker = nil
    self:BindEvent()
end

function InteractionNodeProxy:BindEvent()
    local request = classget(self.source, self.request) as InteractionRequestType
    if request then
        request.Raised = function(sender: BaseType, ...: any)
            self:OnRaised(sender, ...)
        end
    end
end

function InteractionNodeProxy:UnbindEvent()
    local request = classget(self.source, self.request) as InteractionRequestType
    if request then
        request.Raised = nil
    end
end

function InteractionNodeProxy:OnRaised(sender: BaseType, ...: any)
    if self.invoker then
        local invoker = self.invoker as ProxyInvokerType
        invoker:Invoke(sender, ...)
    end
end

function InteractionNodeProxy:SetValue(value: BaseType)
    self.invoker = value
end

function InteractionNodeProxy:dtor()
end

return InteractionNodeProxy