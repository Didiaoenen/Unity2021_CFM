local Misc = require("LuaFramework.Common.Misc")
local UnityPropertyProxy = require("LuaFramework.Binding.Proxy.Targets.UGUI.UnityPropertyProxy")
local UnityEventProxy = require("LuaFramework.Binding.Proxy.Targets.UGUI.UnityEventProxy")

local UnityTargetProxyFactory = class("UnityTargetProxyFactory") as UnityTargetProxyFactoryType

function UnityTargetProxyFactory:ctor()
    
end

function UnityTargetProxyFactory:CreateProxy(target: LuaObject, description: BindingDescriptionType): TargetProxyBaseType
    local memberInfo = classget(target, description.TargetName)

    local updateTrigger: any = nil
    if description.UpdateTrigger and description.UpdateTrigger ~= "" then
        updateTrigger = classget(target, description.UpdateTrigger)
        assert(updateTrigger, "")
    end

    if "userdata" == type(memberInfo) then
        if Misc.CSType.UnityEventBaseT:IsAssignableFrom((memberInfo as CS.System.Object):GetType()) then
            return UnityEventProxy.new(target, description.TargetName) as TargetProxyBaseType
        else
            if not updateTrigger then
                return
            end
        end
    else
        if not updateTrigger then
            return
        end
        
        return UnityPropertyProxy.new(target, description.TargetName, description.UpdateTrigger) as TargetProxyBaseType
    end
end

function UnityTargetProxyFactory:dtor()
end

return UnityTargetProxyFactory