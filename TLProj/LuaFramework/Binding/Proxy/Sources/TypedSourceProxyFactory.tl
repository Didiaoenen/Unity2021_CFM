

require("OOP.Class")

local ISourceProxyFactory = require("LuaFramework.Binding.Proxy.Sources.ISourceProxyFactory")

local TypedSourceProxyFactory = class("TypedSourceProxyFactory", ISourceProxyFactory) as TypedSourceProxyFactoryType

function TypedSourceProxyFactory:ctor()
    
end

function TypedSourceProxyFactory:IsSupported(description: SourceDescriptionType): boolean
    return true
end

function TypedSourceProxyFactory:CreateProxy(source: ObjectType, description: SourceDescriptionType): SourceProxyBaseType
    if not self:IsSupported(description) then
        return
    end

    local proxy: OutProxyType = {}
    if self:TryCreateProxy(source, description, proxy) then
        return proxy.ref
    end

    return proxy.ref
end

function TypedSourceProxyFactory:TryCreateProxy(source: ObjectType, description: SourceDescriptionType, proxy: OutProxyType): boolean
    
end

function TypedSourceProxyFactory:dtor()
end

return TypedSourceProxyFactory