local ProxyInvoker = class("ProxyInvoker") as ProxyInvokerType

function ProxyInvoker.get:ProxyMethodInfo(): string
    return self.proxyMethodInfo
end

function ProxyInvoker:ctor(target: LuaObject, proxyMethodInfo: string)
    self.target = target
    self.proxyMethodInfo = proxyMethodInfo
end

function ProxyInvoker:Invoke(...: any): any...
    local functionType = classget(self.target, self.proxyMethodInfo) as function
    return functionType(self.target, ...)
end

return ProxyInvoker