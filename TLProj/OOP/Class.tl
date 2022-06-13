global class = function(...: any): any...
    print(...)
end

global setcallback = function(...: any): any...
    print(...)
end

global bindevent = function(...: any): any...
    print(...)
end

global split = function(...: any): {string}
    print(...)
end


----------------------------------------------------------

----------------------------------------------------------

require("LuaFramework.Views.WindowEnum")
require("LuaFramework.Binding.BindingMode")

---------------------------------------------------------
global Type = require("OOP.Type")
global ObjectType = require("OOP.ObjectType")


global EventHandlerType = require("OOP.LuaFramework.EventHandlerType")
global UnityEventType = require("OOP.LuaFramework.UnityEventType")
global TableExtentionType = require("OOP.LuaFramework.Common.TableExtentionType")
global StringExtensionType = require("OOP.LuaFramework.Common.StringExtensionType")
global InteractionEventArgsType = require("OOP.LuaFramework.Interactivity.InteractionEventArgsType")
global InteractionRequestType = require("OOP.LuaFramework.Interactivity.InteractionRequestType")
global CommandBaseType = require("OOP.LuaFramework.Commands.CommandBaseType")
global SimpleCommandType = require("OOP.LuaFramework.Commands.SimpleCommandType")
global AbstractBindingType = require("OOP.LuaFramework.Binding.AbstractBindingType")
global BindingDescriptionType = require("OOP.LuaFramework.Binding.BindingDescriptionType")
global BindingExtensionType = require("OOP.LuaFramework.Binding.BindingExtensionType")
global BindingType = require("OOP.LuaFramework.Binding.BindingType")
global UnityProxyRegisterType = require("OOP.LuaFramework.Binding.UnityProxyRegisterType")
global INotifyPropertyChangedType = require("OOP.LuaFramework.Binding.INotifyPropertyChangedType")
global PropertyChangedEventArgsType = require("OOP.LuaFramework.Binding.PropertyChangedEventArgsType")
global StandardBinderType = require("OOP.LuaFramework.Binding.Binders.StandardBinderType")
global BindingBuilderType = require("OOP.LuaFramework.Binding.Builder.BindingBuilderType")
global BindingBuilderBaseType = require("OOP.LuaFramework.Binding.Builder.BindingBuilderBaseType")
global BindingSetType = require("OOP.LuaFramework.Binding.Builder.BindingSetType")
global BindingSetBaseType = require("OOP.LuaFramework.Binding.Builder.BindingSetBaseType")
global BindingContextType = require("OOP.LuaFramework.Binding.Contexts.BindingContextType")
global PathParseType = require("OOP.LuaFramework.Binding.Paths.PathParseType")
global ProxyInvokerType = require("OOP.LuaFramework.Binding.Reflection.ProxyInvokerType")
global INotifiableType = require("OOP.LuaFramework.Binding.Proxy.INotifiableType")
global IModifiableType = require("OOP.LuaFramework.Binding.Proxy.IModifiableType")
global IObtainableType = require("OOP.LuaFramework.Binding.Proxy.IObtainableType")
global BindingProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.BindingProxyBaseType")
global OutProxyType = require("OOP.LuaFramework.Binding.Proxy.OutProxyType")
global PriorityFactoryPairType = require("OOP.LuaFramework.Binding.Proxy.PriorityFactoryPairType")
global ISourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.ISourceProxyFactoryType")
global NotifiableSourceProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Sources.NotifiableSourceProxyBaseType")
global SourceDescriptionType = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceDescriptionType")
global SourceProxyBaseType = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyBaseType")
global SourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.SourceProxyFactoryType")
global TypedSourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.TypedSourceProxyFactoryType")
global UniversalNodeProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.UniversalNodeProxyFactoryType")
global INodeProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.INodeProxyFactoryType")
global InteractionNodeProxyType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.InteractionNodeProxyType")
global MethodNodeProxyType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.MethodNodeProxyType")
global ObjectSourceDescriptionType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.ObjectSourceDescriptionType")
global ObjectSourceProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.ObjectSourceProxyFactoryType")
global PropertyNodeProxyType = require("OOP.LuaFramework.Binding.Proxy.Sources.Object.PropertyNodeProxyType")
global ITargetProxyFactoryType = require("OOP.LuaFramework.Binding.Proxy.Targets.ITargetProxyFactoryType")
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
global MessengerType = require("OOP.LuaFramework.Messenging.MessengerType")
global ObservableObjectType = require("OOP.LuaFramework.Observables.ObservableObjectType")
global ExecutorsExtType = require("OOP.LuaFramework.CSharpObject.ExecutorsExtType")
global AsyncResultExtType = require("OOP.LuaFramework.CSharpObject.AsyncResultExtType")



----------------------------------------------------------
global FunctionsType = require("OOP.LuaFramework.FunctionsType")




----------------------------------------------------------

global PathParse: PathParseType
global Functions: FunctionsType
global WindowManager: WindowManagerType
global UnityProxyRegister: UnityProxyRegisterType



----------------------------------------------------------
local ExecutorsType = require("OOP.ExecutorsType")
local AsyncResultType = require("OOP.AsyncResultType")




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

global record CSharpType
    Execution: Execution
    Asynchronous: Asynchronous
end

global record Execution
    Executors: ExecutorsType
    
end
 
global record Asynchronous
    AsyncResult: AsyncResultType
end