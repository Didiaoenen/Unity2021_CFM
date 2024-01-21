local ValueTargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.ValueTargetProxyBase")
local UnityProxyRegister = require("LuaFramework.Binding.UnityProxyRegister")
local TableOprationMode = require("LuaFramework.Binding.TableOprationMode")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local PropertyTargetProxy = class("PropertyTargetProxy", ValueTargetProxyBase) as PropertyTargetProxyType

function PropertyTargetProxy.get:DefaultMode(): number
    return BindingMode.TwoWay
end

function PropertyTargetProxy:ctor(target: LuaObject, propertyInfo: string)
    ValueTargetProxyBase.ctor(self, target)
    self.propertyInfo = propertyInfo
end

function PropertyTargetProxy:DoSubscribeForValueChange(target: LuaObject)
    local targetNotify = (target as INotifyPropertyChangedType)
    targetNotify.PropertyChanged = {opt = TableOprationMode.Add, save = self.propertyInfo, this = self, callback = self.OnPropertyChanged}
end

function PropertyTargetProxy:DoUnsubscribeForValueChange(target: LuaObject)
    local targetNotify = (target as INotifyPropertyChangedType)
    targetNotify.PropertyChanged = {opt = TableOprationMode.Remove, save = self.propertyInfo}
end

function PropertyTargetProxy:OnPropertyChanged(sender: LuaObject, e: PropertyChangedEventArgsType)
    assert({sender, e}, "")
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

    local proxyInfo = UnityProxyRegister.Instance:Get(self.propertyInfo)
    if proxyInfo then
        return proxyInfo.getter(target)
    else
        return classget(self.target, self.propertyInfo)
    end
end

function PropertyTargetProxy:SetValue(value)
    local target = self.target
    if nil == target then
        return
    end

    local proxyInfo = UnityProxyRegister.Instance:Get(self.propertyInfo)
    if proxyInfo then
        proxyInfo.setter(target, value)
    else
        classset(self.target, self.propertyInfo, value)
    end
end

function PropertyTargetProxy:dtor()
end

return PropertyTargetProxy