local PropertyTargetProxy = require("LuaFramework.Binding.Proxy.Targets.Universal.PropertyTargetProxy")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local UnityFieldOrPropertyProxy = class("UnityFieldOrPropertyProxy", PropertyTargetProxy) as UnityPropertyProxyType

function UnityFieldOrPropertyProxy.get:DefaultMode(): number
    return BindingMode.TwoWay
end

function UnityFieldOrPropertyProxy:ctor(target: LuaObject, propertyInfo: string, unityEvent: string)
    PropertyTargetProxy.ctor(self, target, propertyInfo)
    self.unityEvent = unityEvent
end

function UnityFieldOrPropertyProxy:DoSubscribeForValueChange(target: LuaObject)
    assert({target}, "")
    local unityEvent = classget(self.target, self.unityEvent) as CS.UnityEngine.Events.UnityEvent
    if unityEvent then
        unityEvent:AddListener(bindevent(self, self.OnValueChanged))
    end
end

function UnityFieldOrPropertyProxy:DoUnsubscribeForValueChange()
    local unityEvent = classget(self.target, self.unityEvent) as CS.UnityEngine.Events.UnityEvent
    if unityEvent then
        unityEvent:RemoveAllListeners()
    end
end

function UnityFieldOrPropertyProxy:OnValueChanged(...: any)
    assert({...}, "")
    self:RaiseValueChanged()
end

function UnityFieldOrPropertyProxy:dtor()
    
end

return UnityFieldOrPropertyProxy