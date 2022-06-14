

require("OOP.Class")

local ITargetProxyFactory = require("LuaFramework.Binding.Proxy.Targets.ITargetProxyFactory")

local TargetProxyFactory = class("TargetProxyFactory", ITargetProxyFactory) as TargetProxyFactoryType

function TargetProxyFactory:dtor()
    self.factories = {}
end

function TargetProxyFactory:CreateProxy(target: BaseType, description: BindingDescriptionType): TargetProxyBaseType
    local proxy: OutProxyType = {}
    if self:TryCreateProxy(target, description, proxy) then
        return proxy.ref as TargetProxyBaseType
    end
end

function TargetProxyFactory:TryCreateProxy(target: BaseType, description: BindingDescriptionType, proxy: OutProxyType): boolean
    for _, v in ipairs(self.factories) do
        local factory = v.factory
        if factory then
            proxy.ref = (factory as ITargetProxyFactoryType):CreateProxy(target, description) as BaseType
            if proxy.ref then
                return true
            end
        end
    end
    return false
end

function TargetProxyFactory:Register(factory: ITargetProxyFactoryType, priority: number)
    if nil == factory then
        return        
    end

    table.insert(self.factories, {factory = factory, priority = priority or 100})
    table.sort(self.factories, function (a: PriorityFactoryPairType, b: PriorityFactoryPairType): boolean
        return a.priority > b.priority
    end)
end

function TargetProxyFactory:Unregister(factory: ITargetProxyFactoryType)
    if not factory then
        return
    end

    for i, v in ipairs(self.factories) do
        if v.factory == factory as BaseType then
            table.remove(self.factories, i)
            return
        end
    end
end

function TargetProxyFactory:dtor()
end

return TargetProxyFactory