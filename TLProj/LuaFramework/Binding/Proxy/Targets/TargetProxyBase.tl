

require("OOP.Class")

local BindingProxyBase = require("LuaFramework.Binding.Proxy.BindingProxyBase")

local TargetProxyBase = class("TargetProxyBase", BindingProxyBase) as TargetProxyBaseType

function TargetProxyBase.get:Target(): ObjectType
    return self.target
end

function TargetProxyBase.get:DefaultMode(): number
    return BindingMode.OneWay
end

function TargetProxyBase:ctor(target: ObjectType)
    assert(target, "")
    self.target = target
    self.targetName = ""
end

function TargetProxyBase:IsAlive(target: ObjectType): boolean
    print(target)
    return true
end

function TargetProxyBase:dtor()
end

return TargetProxyBase