require("OOP.Class")

local NotifiableSourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.NotifiableSourceProxyBase")
local INotifyPropertyChanged = require("LuaFramework.Binding.INotifyPropertyChanged")
local UnityProxyRegister = require("LuaFramework.Binding.UnityProxyRegister")
local IObtainable = require("LuaFramework.Binding.Proxy.IObtainable")
local IModifiable = require("LuaFramework.Binding.Proxy.IModifiable")
local INotifiable = require("LuaFramework.Binding.Proxy.INotifiable")

local PropertyNodeProxy = class("PropertyNodeProxy", NotifiableSourceProxyBase, IObtainable, IModifiable, INotifiable) as PropertyNodeProxyType

function PropertyNodeProxy:ctor(source: LuaObject, propertyInfo: string)
    NotifiableSourceProxyBase.ctor(self, source)
    self.propertyInfo = propertyInfo

    if nil == source then
        return
    end

    if self.source.is and self.source.is(INotifyPropertyChanged) then
        local sourceNotify = self.source as INotifyPropertyChangedType
        sourceNotify.PropertyChanged = {this = self, callback = self.OnPropertyChanged}
    end    
end

function PropertyNodeProxy:OnPropertyChanged(sender: LuaObject, e: PropertyChangedEventArgsType)
    local name = e.PropertyName
    if name and name ~= "" and name == self.propertyInfo then
        self:RaiseValueChanged()
    end
end

function PropertyNodeProxy:GetValue(): any
    local source = self.source
    if nil == source then
        return
    end

    local proxyInfo = UnityProxyRegister.Instance:Get(self.propertyInfo)
    if proxyInfo then
        return proxyInfo.getter(source)
    else
        return classget(self.source, self.propertyInfo)
    end
end

function PropertyNodeProxy:SetValue(value: any)
    local source = self.source
    if nil == source then
        return
    end

    local proxyInfo = UnityProxyRegister.Instance:Get(self.propertyInfo)
    if proxyInfo then
        proxyInfo.setter(source, value)
    else
        classset(self.source, self.propertyInfo, value)
    end
end

function PropertyNodeProxy:dtor()
end

return PropertyNodeProxy