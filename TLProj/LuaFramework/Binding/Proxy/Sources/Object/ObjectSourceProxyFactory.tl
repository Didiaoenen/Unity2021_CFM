

require("OOP.Class")

local TypedSourceProxyFactory = require("LuaFramework.Binding.Proxy.Sources.TypedSourceProxyFactory")

local ObjectSourceProxyFactory = class("ObjectSourceProxyFactory", TypedSourceProxyFactory) as ObjectSourceProxyFactoryType

function ObjectSourceProxyFactory:ctor()
    TypedSourceProxyFactory.ctor(self)
    self.factories = {}        
end

function ObjectSourceProxyFactory:TryCreateProxy(source: ObjectType, description: SourceDescriptionType, proxy: OutProxyType): boolean
    local path = (description as ObjectSourceDescriptionType).Path
    assert(#(path) > 0)

    if #path == 1 then
        proxy.ref = self:Create(source, path)
        if proxy.ref then
            return true
        end
        return false
    end

    return true
end

function ObjectSourceProxyFactory:Create(source: ObjectType, path: {string}): SourceProxyBaseType
    local proxy: OutProxyType = {}
    for _, v in ipairs(self.factories) do
        local factory = v.factory
        if factory then
            proxy.ref = (factory as INodeProxyFactoryType):Create(source, path)
            if proxy.ref then
                return proxy.ref
            end
        end
    end

    return proxy.ref
end

function ObjectSourceProxyFactory:Register(factory: PriorityFactoryPairType, priority: number)
    if not factory then
        return        
    end

    table.insert(self.factories, {factory = factory, priority = priority or 100})
    table.sort(self.factories, function (a: PriorityFactoryPairType, b: PriorityFactoryPairType): boolean
        return a.priority > b.priority
    end)
end

function ObjectSourceProxyFactory:Unregister(factory: INodeProxyFactoryType)
    if not factory then
        return
    end

    for i, v in ipairs(self.factories) do
        if v.factory == factory as ObjectType then
            table.remove(self.factories, i)
            return
        end
    end
end

function ObjectSourceProxyFactory:dtor()

end

return ObjectSourceProxyFactory