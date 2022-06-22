require("OOP.Class")

local BindingProxyBase = require("LuaFramework.Binding.Proxy.BindingProxyBase")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local TargetProxyBase = class("TargetProxyBase", BindingProxyBase) as TargetProxyBaseType

function TargetProxyBase.get:Target(): LuaObject
    return self.target
end

function TargetProxyBase.get:DefaultMode(): number
    return BindingMode.OneWay
end

function TargetProxyBase:ctor(target: LuaObject)
    assert(target, "")
    self.target = target
    self.targetName = ""
end

function TargetProxyBase:IsAlive(target: LuaObject): boolean
    return true
end

function TargetProxyBase:dtor()
end

return TargetProxyBase