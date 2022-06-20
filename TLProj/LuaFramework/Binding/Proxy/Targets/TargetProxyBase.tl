

require("OOP.Class")

local BindingProxyBase = require("LuaFramework.Binding.Proxy.BindingProxyBase")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local TargetProxyBase = class("TargetProxyBase", BindingProxyBase) as TargetProxyBaseType

function TargetProxyBase.get:Target(): BaseType
    return self.target
end

function TargetProxyBase.get:DefaultMode(): number
    return BindingMode.OneWay
end

function TargetProxyBase:ctor(target: BaseType)
    assert(target, "")
    self.target = target
    self.targetName = ""
end

function TargetProxyBase:IsAlive(target: BaseType): boolean
    print(target)
    return true
end

function TargetProxyBase:dtor()
end

return TargetProxyBase