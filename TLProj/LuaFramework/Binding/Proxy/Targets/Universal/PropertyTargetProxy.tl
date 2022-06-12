

require("OOP.Class")

local ValueTargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.ValueTargetProxyBase")

local PropertyTargetProxy = class("PropertyTargetProxy", ValueTargetProxyBase) as PropertyTargetProxyType

function PropertyTargetProxy.get:DefaultMode(): number
    return BindingMode.TwoWay
end

function PropertyTargetProxy:ctor(target: ObjectType, propertyInfo: string)
    ValueTargetProxyBase.ctor(self, target)
    self.propertyInfo = propertyInfo
end

function PropertyTargetProxy:DoSubscribeForValueChange(target: ObjectType)
    (target as INotifyPropertyChangedType).PropertyChanged = {this = self, callback = self.OnPropertyChanged}
end

function PropertyTargetProxy:DoUnsubscribeForValueChange(target: ObjectType)
    (target as INotifyPropertyChangedType).PropertyChanged = {this = self}
end

function PropertyTargetProxy:OnPropertyChanged(sender: ObjectType, e: PropertyChangedEventArgsType)
    local name = e.PropertyName
    if not name or name == "" or name == self.propertyInfo then
        local target = self.target
        if nil == target then
            return
        end

        self:RaiseValueChanged()
    end
end

function PropertyTargetProxy:GetValue(): any
    local target = self.target
    if nil == target then
        return
    end

    local proxyInfo = UnityProxyRegister:Get(self.propertyInfo)
    if proxyInfo then
        return proxyInfo.getter(target)
    else
        return Functions.ClassGet(self.target, self.propertyInfo)
    end
end

function PropertyTargetProxy:SetValue(value)
    local target = self.target
    if not target then
        return
    end

    local proxyInfo = UnityProxyRegister:Get(self.propertyInfo)
    if proxyInfo then
        proxyInfo.setter(target, value)
    else
        Functions.ClassSet(self.target, self.propertyInfo, value)
    end
end

function PropertyTargetProxy:dtor()
end

return PropertyTargetProxy