local NotifiableSourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.NotifiableSourceProxyBase")
local INotifyPropertyChanged = require("LuaFramework.Binding.INotifyPropertyChanged")
local UnityProxyRegister = require("LuaFramework.Binding.UnityProxyRegister")
local TableOprationMode = require("LuaFramework.Binding.TableOprationMode")

local PropertyNodeProxy = class("PropertyNodeProxy", NotifiableSourceProxyBase) as PropertyNodeProxyType

function PropertyNodeProxy:ctor(source: LuaObject, propertyInfo: string)
    NotifiableSourceProxyBase.ctor(self, source)
    self.propertyInfo = propertyInfo

    if nil == source then
        return
    end

    if self.source.is and self.source.is(INotifyPropertyChanged) then
        local sourceNotify = self.source as INotifyPropertyChangedType
        sourceNotify.PropertyChanged = {opt = TableOprationMode.Add, save = self.propertyInfo, this = self, callback = self.OnPropertyChanged}
    end    
end

function PropertyNodeProxy:OnPropertyChanged(sender: LuaObject, e: PropertyChangedEventArgsType)
    assert({sender, e}, "")
    local name = e.PropertyName
    if not name or name == "" or name == self.propertyInfo then
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