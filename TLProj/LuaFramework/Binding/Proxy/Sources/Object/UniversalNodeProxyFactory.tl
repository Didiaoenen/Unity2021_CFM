require("OOP.Class")

local INodeProxyFactory = require("LuaFramework.Binding.Proxy.Sources.Object.INodeProxyFactory")
local MethodNodeProxy = require("LuaFramework.Binding.Proxy.Sources.Object.MethodNodeProxy")
local PropertyNodeProxy = require("LuaFramework.Binding.Proxy.Sources.Object.PropertyNodeProxy")
local InteractionNodeProxy = require("LuaFramework.Binding.Proxy.Sources.Object.InteractionNodeProxy")

local UniversalNodeProxyFactory = class("UniversalNodeProxyFactory", INodeProxyFactory) as UniversalNodeProxyFactoryType

function UniversalNodeProxyFactory:ctor()

end

function UniversalNodeProxyFactory:Create(source: LuaObject, token: PathTokenType): SourceProxyBaseType
    local node = token.Current
    if nil == source then
        return
    end

    return self:CreateProxy(source, node)
end

function UniversalNodeProxyFactory:CreateProxy(source: LuaObject, node: string): SourceProxyBaseType
    local memberInfo = classget(source, node)

    if "function" == type(memberInfo) then
        return MethodNodeProxy.new(source, node) as SourceProxyBaseType
    elseif "userdata" == type(memberInfo) then
        return PropertyNodeProxy.new(source, node) as SourceProxyBaseType
    else
        if "table" == type(memberInfo) then
            if (memberInfo as LuaObject).is(InteractionNodeProxy) then
                return InteractionNodeProxy.new(source, node) as SourceProxyBaseType
            end
        end

        return PropertyNodeProxy.new(source, node) as SourceProxyBaseType
    end
end

function UniversalNodeProxyFactory:dtor()
end

return UniversalNodeProxyFactory
