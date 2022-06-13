local Object = require("OOP.ObjectType")
local SimpleCommand = require("OOP.LuaFramework.Commands.SimpleCommandType")
local ProxyInvoker = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")

local record UnityEventProxy
    target: Object
    unityEvent: string

    invoker: ProxyInvoker
    command: SimpleCommand

    new: function(...: any): UnityEventProxy
    ctor: function
    dtor: function
    BindEvent: function(UnityEventProxy)
    UnbindEvent: function(UnityEventProxy)
    IsValidInvoker: function(UnityEventProxy, ProxyInvoker)
    OnEvent: function(UnityEventProxy, ...: any)
end

return UnityEventProxy