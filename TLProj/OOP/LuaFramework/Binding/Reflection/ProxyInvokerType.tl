local Object = require("OOP.ObjectType")

local record Get
    proxyMethodInfo: string
    ProxyMethodInfo: function(Get): string
end

local record ProxyInvoker
    get: Get

    target: Object
    proxyMethodInfo: string

    new: function(...: any): ProxyInvoker
    ctor: function
    dtor: function
    Invoke: function(ProxyInvoker, ...: any): any...
    ProxyMethodInfo: function(ProxyInvoker): string
end

return ProxyInvoker