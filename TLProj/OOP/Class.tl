global class = function(...: any): any...
    print(...)
end

global setcallback = function(...: any): any...
    print(...)
end

global bindevent = function(...: any): any...
    print(...)
end


----------------------------------------------------------

----------------------------------------------------------

require("OOP.Define")
require("LuaFramework.Views.WindowEnum")
require("OOP.LuaFramework.Binding.BindingModeDefine")

---------------------------------------------------------
global Type = require("OOP.Type")
global ObjectType = require("OOP.ObjectType")


global EventHandlerType = require("OOP.LuaFramework.EventHandlerType")
global UnityEventType = require("OOP.LuaFramework.UnityEventType")
global InteractionEventArgsType = require("OOP.LuaFramework.Interactivity.InteractionEventArgsType")
global InteractionRequestType = require("OOP.LuaFramework.Interactivity.InteractionRequestType")
global CommandBaseType = require("OOP.LuaFramework.Commands.CommandBaseType")
global SimpleCommandType = require("OOP.LuaFramework.Commands.SimpleCommandType")
global AbstractBindingType = require("OOP.LuaFramework.Binding.AbstractBindingType")
global BindingDescriptionType = require("OOP.LuaFramework.Binding.BindingDescriptionType")
global UnityProxyRegisterType = require("OOP.LuaFramework.Binding.UnityProxyRegisterType")
global ProxyInvokerType = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")
global INotifiableType = require("OOP.LuaFramework.Binding.Proxy.INotifiableType")
global INotifiableType = require("OOP.LuaFramework.Binding.Proxy.INotifiableType")
global IObtainableType = require("OOP.LuaFramework.Binding.Proxy.IObtainableType")
global BindingProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.BindingProxyBaseType")
global OutProxyType = require("OOP.LuaFramework.Binding.Proxy.OutProxyType")
global PriorityFactoryPairType = require("OOP.LuaFramework.Binding.Proxy.PriorityFactoryPairType")
global PropertyChangedEventArgsType = require("OOP.LuaFramework.Binding.Proxy.PropertyChangedEventArgsType")
global ISourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.ISourceProxyFactoryType")
global NotifiableSourceProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Sources.NotifiableSourceProxyBaseType")
global SourceDescriptionType = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")
global SourceProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
global SourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyFactoryType")
global TypedSourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.TypedSourceProxyFactoryType")
global UniversalNodeProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.UniversalNodeProxyFactoryType")
global INodeProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.INodeProxyFactoryType")
global InteractionNodeProxyType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.InteractionNodeProxyType")
global MethodNodeProxyType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.MethodNodeProxyType")
global ObjectSourceDescriptionType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.ObjectSourceDescriptionType")
global ObjectSourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.ObjectSourceProxyFactoryType")
global PropertyNodeProxyType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.PropertyNodeProxyType")
global TargetProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyBaseType")
global TargetProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Targets.TargetProxyFactoryType")
global ValueTargetProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Targets.ValueTargetProxyBaseType")
global UnityEventProxyType = require("OOP.LuaFramework.Binding.Proxy.Targets.UGUI.UnityEventProxyType")
global UnityEventProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Targets.UGUI.UnityEventProxyBaseType")
global UnityPropertyProxyType = require("OOP.LuaFramework.Binding.Proxy.Targets.UGUI.UnityPropertyProxyType")
global UnityTargetProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Targets.UGUI.UnityTargetProxyFactoryType")
global EventTargetProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Targets.Universal.EventTargetProxyBaseType")
global InteractionTargetProxyType = require("OOP.LuaFramework.Binding.Proxy.Targets.Universal.InteractionTargetProxyType")
global MethodTargetProxyType = require("OOP.LuaFramework.Binding.Proxy.Targets.Universal.MethodTargetProxyType")
global PropertyTargetProxyType = require("OOP.LuaFramework.Binding.Proxy.Targets.Universal.PropertyTargetProxyType")
global UniversalTargetProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Targets.Universal.UniversalTargetProxyFactoryType")
global LayerType = require("OOP.LuaFramework.Views.LayerType")
global WindowManagerType = require("OOP.LuaFramework.Views.WindowManagerType")
global INotifyPropertyChangedType = require("OOP.LuaFramework.Views.INotifyPropertyChangedType")
global ExecutorsType = require("OOP.LuaFramework.CSharpObject.ExecutorsType")



----------------------------------------------------------
global FunctionsType = require("OOP.LuaFramework.FunctionsType")




----------------------------------------------------------

global Functions: FunctionsType
global WindowManager: WindowManagerType
global UnityProxyRegister: UnityProxyRegisterType


global Executors: ExecutorsType


----------------------------------------------------------



global typeof = function(...: any): Type
    print(...)
end

global record CS
    UnityEngine: UnityEngine
end

local record UnityEngine
    Events: Events
end

local record Events
    UnityEventBase: UnityEventBase
end

local record UnityEventBase

end