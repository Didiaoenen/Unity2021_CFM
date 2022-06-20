

require("OOP.Class")

local UnityProxyRegister = class("UnityProxyRegister") as UnityProxyRegisterType

function UnityProxyRegister:ctor()
    self.registerProxies = {}
    self:Initialize()
end

function UnityProxyRegister:Initialize()
    self.registerProxies["activeSelf"] = {
        getter = function (target: GameObject): boolean return target.activeSelf end,
        setter = function (target: GameObject, value: boolean) target:SetActive(value) end,
    }
    self.registerProxies["parent"] = {
        getter = function (target: Transform): Transform return target.parent end,
        setter = function (target: Transform, value: Transform) target:SetParent(value) end,
    }
end

function UnityProxyRegister:__singleton(): UnityProxyRegisterType
    return UnityProxyRegister.new()
end

function UnityProxyRegister:Get(memberInfo: string): UnityProxyRegisterType.Convert
    return self.registerProxies[memberInfo]
end

function UnityProxyRegister:dtor()
end

return UnityProxyRegister
