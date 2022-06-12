

require("OOP.Class")

local MethodTargetProxy = require("LuaFramework.Binding.Proxy.Targets.Universal.MethodTargetProxy")
local PropertyTargetProxy = require("LuaFramework.Binding.Proxy.Targets.Universal.PropertyTargetProxy")

local UniversalTargetProxyFactory = class("UniversalTargetProxyFactory") as UniversalTargetProxyFactoryType

function UniversalTargetProxyFactory:ctor()
    
end

function UniversalTargetProxyFactory:CreateProxy(target: ObjectType, description: BindingDescriptionType): TargetProxyBaseType
    local memberInfo = Functions.ClassGet(target, description.TargetName)

    if "function" == type(memberInfo) then
        return MethodTargetProxy.new(target, description.TargetName) as TargetProxyBaseType
    elseif "userdata" == type(memberInfo) then
        return PropertyTargetProxy.new(target, description.TargetName) as TargetProxyBaseType
    else
        if "table" == type(memberInfo) then

        end
        return PropertyTargetProxy.new(target, description.TargetName) as TargetProxyBaseType
    end
end

function UniversalTargetProxyFactory:dtor()
end

return UniversalTargetProxyFactory