local Object = require("OOP.ObjectType")
local ProxyInvoker = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")

local record Get
    DefaultMode: function

    methodInfo: string
    ProxyMethodInfo: function
end

local record MethodTargetProxy
    get: Get 

    DefaultMode: number

    methodInfo: string
    ProxyMethodInfo: string

    target: Object
    invoker: ProxyInvoker
    
    new: function
    ctor: function
    dtor: function
    GetValue: function(MethodTargetProxy): any
    Invoke: function(MethodTargetProxy, Object, ...:any)
end

return MethodTargetProxy