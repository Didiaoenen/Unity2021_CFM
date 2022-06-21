

require("OOP.Class")

local TypedSourceProxyFactory = require("LuaFramework.Binding.Proxy.Sources.TypedSourceProxyFactory")
local INodeProxyFactory = require("LuaFramework.Binding.Proxy.Sources.Object.INodeProxyFactory")

local ObjectSourceProxyFactory = class("ObjectSourceProxyFactory", TypedSourceProxyFactory, INodeProxyFactory) as ObjectSourceProxyFactoryType

function ObjectSourceProxyFactory:ctor()
    TypedSourceProxyFactory.ctor(self)
    self.factories = {}        
end

function ObjectSourceProxyFactory:TryCreateProxy(source: LuaObject, description: SourceDescriptionType, proxy: OutProxyType): boolean
    local path = (description as ObjectSourceDescriptionType).Path
    assert(#(path) > 0)

    if #path == 1 then
        proxy.ref = self:Create(source, path) as LuaObject
        if proxy.ref then
            return true
        end
        return false
    end

    return true
end

function ObjectSourceProxyFactory:Create(source: LuaObject, path: {string}): SourceProxyBaseType
    local proxy: OutProxyType = {}
    for _, v in ipairs(self.factories) do
        local factory = v.factory
        if factory then
            proxy.ref = (factory as INodeProxyFactoryType):Create(source, path) as LuaObject
            if proxy.ref then
                return proxy.ref as SourceProxyBaseType
            end
        end
    end

    return proxy.ref as SourceProxyBaseType
end

function ObjectSourceProxyFactory:Register(factory: INodeProxyFactoryType, priority: number)
    if nil == factory then
        return    
    end

    table.insert(self.factories, {factory = factory, priority = priority or 100})
    table.sort(self.factories, function (a: PriorityFactoryPairType, b: PriorityFactoryPairType): boolean
        return a.priority > b.priority
    end)
end

function ObjectSourceProxyFactory:Unregister(factory: INodeProxyFactoryType)
    if nil == factory then
        return
    end

    for i, v in ipairs(self.factories) do
        if v.factory == factory as LuaObject then
            table.remove(self.factories, i)
            return
        end
    end
end

function ObjectSourceProxyFactory:dtor()
end

return ObjectSourceProxyFactory