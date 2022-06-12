

require("OOP.Class")

local PropertyTargetProxy = require("LuaFramework.Binding.Proxy.Targets.Universal.PropertyTargetProxy")

local UnityPropertyProxy = class("UnityPropertyProxy", PropertyTargetProxy) as UnityPropertyProxyType

function UnityPropertyProxy.get:DefaultMode(): number
    return BindingMode.TwoWay
end

function UnityPropertyProxy:ctor(target: ObjectType, propertyInfo: string, unityEvent: string)
    PropertyTargetProxy.ctor(self, target, propertyInfo)
    self.unityEvent = unityEvent
end

function UnityPropertyProxy:DoSubscribeForValueChange(target: ObjectType)
    local unityEvent = Functions.ClassGet(self.target, self.unityEvent)
    if unityEvent then
        (unityEvent as UnityEventType):AddListener(bindevent(self, self.OnValueChanged))
    end
end

function UnityPropertyProxy:DoUnsubscribeForValueChange()
    local unityEvent = Functions.ClassGet(self.target, self.unityEvent)
    if unityEvent then
        (unityEvent as UnityEventType):RemoveAllListeners()
    end
end

function UnityPropertyProxy:OnValueChanged(...: any)
    self:RaiseValueChanged()
end

function UnityPropertyProxy:dtor()
    
end

return UnityPropertyProxy