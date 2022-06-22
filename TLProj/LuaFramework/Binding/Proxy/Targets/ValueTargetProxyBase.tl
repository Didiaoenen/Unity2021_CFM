require("OOP.Class")

local TargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.TargetProxyBase")

local ValueTargetProxyBase = class("ValueTargetProxyBase", TargetProxyBase) as ValueTargetProxyBaseType

function ValueTargetProxyBase.set:ValueChanged(value: EventHandlerType)
    setcallback(self.valueChanged, value, function ()
        if self.valueChanged and not self.subScribed then
            self:Subscribe()
        end
    end)
end

function ValueTargetProxyBase:ctor(target: LuaObject)
    TargetProxyBase.ctor(self, target)
    self.subScribed = false
    self.valueChanged = {}
end

function ValueTargetProxyBase:Subscribe()
    if self.subScribed then
        return
    end

    local target = self.target
    if nil == target then
        return
    end

    self.subScribed = true
    self:DoSubscribeForValueChange(target)
end

function ValueTargetProxyBase:DoSubscribeForValueChange(target: LuaObject)
end

function ValueTargetProxyBase:Unsubscribe()
    if not self.subScribed then
        return
    end

    local target = self.target
    if nil == target then
        return
    end

    self.subScribed = false
    self:DoUnsubscribeForValueChange(target)
end

function ValueTargetProxyBase:DoUnsubscribeForValueChange(target: LuaObject)
end

function ValueTargetProxyBase:RaiseValueChanged()
    for _, v in ipairs(self.valueChanged) do
        v.callback(v.this)
    end
end

function ValueTargetProxyBase:GetValue(): any
end

function ValueTargetProxyBase:SetValue(value: any)
end

function ValueTargetProxyBase:dtor()
end

return ValueTargetProxyBase