local Object = require("OOP.ObjectType")
local SimpleCommand = require("OOP.LuaFramework.Commands.SimpleCommandType")
local ProxyInvoker = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")

local record Get
    DefaultMode: function
end

local record UnityEventProxyBase
    get: Get

    DefaultMode: number

    target: Object
    unityEvent: string
    interactable: string
    command: SimpleCommand
    invoker: ProxyInvoker

    ctor: function
    dtor: function
    BindEvent: function(UnityEventProxyBase)
    UnbindEvent: function(UnityEventProxyBase)
    IsValidInvoker: function(UnityEventProxyBase, ProxyInvoker)
    SetValue: function(UnityEventProxyBase, any)
    OnExecuteChanged: function(UnityEventProxyBase, Object, ...: any)
    UpdateTargetInteractable: function(UnityEventProxyBase)
    BindCommand: function(UnityEventProxyBase, SimpleCommand)
    UnbindCommand: function(UnityEventProxyBase, SimpleCommand)
end

return UnityEventProxyBase