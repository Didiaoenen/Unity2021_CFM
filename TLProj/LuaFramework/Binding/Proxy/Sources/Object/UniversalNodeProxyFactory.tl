

require("OOP.Class")

local INodeProxyFactory = require("LuaFramework.Binding.Proxy.Sources.Object.INodeProxyFactory")
local MethodNodeProxy = require("LuaFramework.Binding.Proxy.Sources.Object.MethodNodeProxy")
local PropertyNodeProxy = require("LuaFramework.Binding.Proxy.Sources.Object.PropertyNodeProxy")
local InteractionNodeProxy = require("LuaFramework.Binding.Proxy.Sources.Object.InteractionNodeProxy")

local UniversalNodeProxyFactory = class("UniversalNodeProxyFactory", INodeProxyFactory) as UniversalNodeProxyFactoryType

function UniversalNodeProxyFactory:ctor()

end

function UniversalNodeProxyFactory:Create(source: BaseType, path: {string}): SourceProxyBaseType
    local node = path[1]
    if nil == source then
        return
    end

    return self:CreateProxy(source, node)
end

function UniversalNodeProxyFactory:CreateProxy(source: BaseType, node: string): SourceProxyBaseType
    local memberInfo = Functions.ClassGet(source, node)

    if "function" == type(memberInfo) then
        return MethodNodeProxy.new(source, node) as SourceProxyBaseType
    elseif "userdata" == type(memberInfo) then
        return PropertyNodeProxy.new(source, node) as SourceProxyBaseType
    else
        if (memberInfo as BaseType).is(InteractionNodeProxy) then
            return InteractionNodeProxy.new(source, node) as SourceProxyBaseType
        end

        return PropertyNodeProxy.new(source, node) as SourceProxyBaseType
    end
end

function UniversalNodeProxyFactory:dtor()
end

return UniversalNodeProxyFactory
