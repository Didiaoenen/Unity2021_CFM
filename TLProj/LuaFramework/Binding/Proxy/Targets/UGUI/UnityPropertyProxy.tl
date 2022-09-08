local PropertyTargetProxy = require("LuaFramework.Binding.Proxy.Targets.Universal.PropertyTargetProxy")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local UnityPropertyProxy = class("UnityPropertyProxy", PropertyTargetProxy) as UnityPropertyProxyType

function UnityPropertyProxy.get:DefaultMode(): number
    return BindingMode.TwoWay
end

function UnityPropertyProxy:ctor(target: LuaObject, propertyInfo: string, unityEvent: string)
    PropertyTargetProxy.ctor(self, target, propertyInfo)
    self.unityEvent = unityEvent
end

function UnityPropertyProxy:DoSubscribeForValueChange(target: LuaObject)
    assert({target}, "")
    local unityEvent = classget(self.target, self.unityEvent) as CS.UnityEngine.Events.UnityEvent
    if unityEvent then
        unityEvent:AddListener(bindevent(self, self.OnValueChanged))
    end
end

function UnityPropertyProxy:DoUnsubscribeForValueChange()
    local unityEvent = classget(self.target, self.unityEvent) as CS.UnityEngine.Events.UnityEvent
    if unityEvent then
        unityEvent:RemoveAllListeners()
    end
end

function UnityPropertyProxy:OnValueChanged(...: any)
    assert({...}, "")
    self:RaiseValueChanged()
end

function UnityPropertyProxy:dtor()
    
end

return UnityPropertyProxy