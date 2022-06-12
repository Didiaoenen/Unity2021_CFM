local Object = require("OOP.ObjectType")
local SimpleCommand = require("OOP.LuaFramework.Commands.SimpleCommandType")
local ProxyInvoker = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")

local record UnityEventProxy
    target: Object
    unityEvent: string

    invoker: ProxyInvoker
    command: SimpleCommand

    new: function
    ctor: function
    dtor: function
    BindEvent: function(UnityEventProxy)
    UnbindEvent: function(UnityEventProxy)
    IsValidHandler: function(UnityEventProxy, any)
    IsValidInvoker: function(UnityEventProxy, any)
    OnEvent: function(UnityEventProxy, ...: any)
end

return UnityEventProxy