local Object = require("OOP.ObjectType")
local ProxyInvoker = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")

local record MethodNodeProxy
    source: Object
    methodInfo: string
    invoker: ProxyInvoker

    new: function
    ctor: function
    dtor: function
    GetValue: function(MethodNodeProxy): any
end

return MethodNodeProxy