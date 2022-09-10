local IProxyInvoker = class("ProxyInvoker") as IProxyInvokerType

function IProxyInvoker.get:ProxyMethodInfo(): string
    return self.proxyMethodInfo
end

function IProxyInvoker:Invoke(...: any): any
    assert({...}, "")
end

return IProxyInvoker