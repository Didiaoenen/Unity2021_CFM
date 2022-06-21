----------------------------------------------------------
global class = function(...: any): any
    print(...)
end

global setcallback = function(...: any): any...
    print(...)
end

global bindevent = function(...: any): any...
    print(...)
end

global try = function(...: any)
    print(...)
end

global classget = function(cls: any, key: string): any
    print(cls, key)
end

global classset = function(cls: any, key: string, value: any)
    print(cls, key, value)
end

global clone = function(obj: any)
    print(obj)
end

---------------------------------------------------------
global record CS
    CSName: CSName
    CSType: CSType
    CSObject: CSObject
    
    -- System
    record System
        record Type
            IsAssignableFrom: function(Type, Type): boolean
        end
        
        record Object
            name: string
            GetType: function(Object): Type
        end
    end

    -- UnityEngine
    record UnityEngine

        -- Object
        record Object
            name: string
            GetType: function(Object): System.Type
            Instantiate: function(CS.UnityEngine.Object): CS.UnityEngine.Object
            Destroy: function(CS.UnityEngine.Object)
        end
    
        -- ResourceRequest
        record ResourceRequest
            isDone: boolean
            progress: number
            asset: CS.UnityEngine.Object
        end

        -- Resources
        record Resources
            LoadAsync: function(string, any)
        end

        -- WaitForSeconds
        record WaitForSeconds
            metamethod __call: function(WaitForSeconds, ...: any): WaitForSeconds
        end
        
        -- Quaternion
        record Quaternion
            identity: Quaternion
        end

        -- Canvas
        record Canvas
        end

        -- Component
        record Component
            GetComponent: function(Component, CS.System.Type): Component
            AddComponent: function(Component, CS.System.Type): Component
        end
    
        -- Transform
        record Transform
            parent: Transform
        
            SetParent: function(Transform, Transform)
        end

        -- GameObject
        record GameObject
            transform: Transform
            activeSelf: boolean
        
            Find: function(string): GameObject
            SetActive: function(GameObject, boolean)
        end

        -- CanvasGroup
        record CanvasGroup
        end

        -- RectTransform
        record RectTransform
        end

        -- Vector3
        record Vector3
            zero: Vector3
            one: Vector3
        
            metamethod __call: function(Vector3, ...: any): Vector3
        end
        
        -- Vector2
        record Vector2
            zero: Vector2
        end
        
        -- Color
        record Color
            red: Color
        end

        -- Events
        record Events
            -- UnityEventBase
            record UnityEventBase

            end

            -- UnityEventBase
            record UnityEvent
                AddListener: function
                RemoveAllListeners: function
            end
        end

        -- UI
        record UI
            -- Text
            record Text

            end

            -- Button
            record Button

            end

            -- InputField
            record InputField

            end

            -- GraphicRaycaster
            record GraphicRaycaster

            end
        end
    end

    -- 自定义类
end

---------------------------------------------------------
global typeof = function(...: any): CS.System.Type
    print(...)
end

--------------------------------------------------------------
local record CSObject
    record Simple
        record MonoVariableArray
            record VariableArray
                Get: function(VariableArray, string): CS.UnityEngine.Component
            end

            variableArray: VariableArray
        end
    end
    
    record Execution
        record Executors
            WaitWhile: function(function): CS.System.Object
            RunOnMainThread: function(function)
            RunOnCoroutine: function(function): Asynchronous.AsyncResult
            RunOnCoroutineNoReturn: function(function)
        end
    end

    record Asynchronous
        record AsyncResult
            done: boolean
            result: CS.System.Object
            cancellationRequested: boolean
            cancelled: boolean
            SetResult: function(AsyncResult, CS.System.Object)
            SetCancelled: function(AsyncResult)
            Cancel: function(AsyncResult): boolean
            Callbackable: function(AsyncResult): CS.System.Object
            Synchronized: function(AsyncResult): CS.System.Object
            WaitForDone: function(AsyncResult): CS.System.Object
        
            new: function(...: any): AsyncResult
        
            metamethod __call: function(AsyncResult, ...: any): AsyncResult
        end

        record ProgressResult_1_Single
            Result: CS.System.Object
            Progress: number
        
            Callbackable: function(ProgressResult_1_Single): CS.System.Object
            UpdateProgress: function(ProgressResult_1_Single, number)
            SetResult: function(ProgressResult_1_Single, CS.System.Object)
            WaitForDone: function(ProgressResult_1_Single): CS.System.Object
        
            metamethod __call: function(ProgressResult_1_Single, ...: any): ProgressResult_1_Single
        end

        ["ProgressResult`1[System.Single]"]: ProgressResult_1_Single
    end
end

local record CSNameType
    ProgressResult_1_Single: string
end

local CSName: CSNameType = {
    ProgressResult_1_Single = "ProgressResult`1[System.Single]"
}

local record CSTypeType
    ObjectT: CS.System.Type
    CanvasT: CS.System.Type
    TransformT: CS.System.Type
    GameObjectT: CS.System.Type
    CanvasGroupT: CS.System.Type
    RectTransformT: CS.System.Type

    TextT: CS.System.Type
    ButtonT: CS.System.Type
    InputFieldT: CS.System.Type
    GraphicRaycasterT: CS.System.Type

    UnityEventBaseT: CS.System.Type
end

local CSType: CSTypeType = {
    ObjectT = typeof(CS.UnityEngine.Object),
    CanvasT = typeof(CS.UnityEngine.Canvas),
    TransformT = typeof(CS.UnityEngine.Transform),
    GameObjectT = typeof(CS.UnityEngine.GameObject),
    CanvasGroupT = typeof(CS.UnityEngine.CanvasGroup),
    RectTransformT = typeof(CS.UnityEngine.RectTransform),

    TextT = typeof(CS.UnityEngine.UI.Text),
    ButtonT = typeof(CS.UnityEngine.UI.Button),
    InputFieldT = typeof(CS.UnityEngine.UI.InputField),
    GraphicRaycasterT = typeof(CS.UnityEngine.UI.GraphicRaycaster),

    UnityEventBaseT = typeof(CS.UnityEngine.Events.UnityEventBase),
}

----------------------------------------------------------
global record WindowEnumType

    record WindowTypeType
        NONE: number
        FULL: number
        POPUP: number
        DIALOG: number
        PROGRESS: number
        QUEUED_POPUP: number
    end

    WindowType: WindowTypeType

    record WindowStateType
        NONE: number
        CREATE_BEGIN: number
        CREATE_END: number
        ENTER_ANIMATION_BEGIN: number
        VISIBLE: number
        ENTER_ANIMATION_END: number
        ACTIVATION_ANIMATION_BEGIN: number
        ACTIVATED: number
        ACTIVATION_ANIMATION_END: number
        PASSIVATION_ANIMATION_BEGIN: number
        PASSIVATED: number
        PASSIVATION_ANIMATION_END: number
        EXIT_ANIMATION_BEGIN: number
        INVISIBLE: number
        EXIT_ANIMATION_END: number
        DISMISS_BEGIN: number
        DISMISS_END: number
    end

    WindowState: WindowStateType

    record ActionTypeType
        None: number
        Hide: number
        Dismiss: number
    end

    ActionType: ActionTypeType
end

global record BindingModeType
    Default: number         
    TwoWay: number          
    OneWay: number          
    OneTime: number         
    OneWayToSource: number  
end

----------------------------------------------------------
global record LuaObject
    is: function(any): boolean
end

global record LayerDataType
    WindowType: number
    OrderInLayer: number
    OrderStep: number
end

global record PriorityFactoryPairType
    factory: LuaObject
    priority: number
end

global record EventHandlerType
    this: LuaObject
    callback: function
    args: PropertyChangedEventArgsType
end

global record SetType
    name: string
end

----------------------------------------------------------
global record AbstractBindingType
    record get
        dataContext: LuaObject
        DataContext: function
    
        target: LuaObject
        Target: function

        IsAlive: function(set, LuaObject): boolean
    end
    
    record set
        dataContext: LuaObject
        DataContext: function

        OnDataContextChanged: function(set)
    end

    dataContext: LuaObject
    DataContext: LuaObject
    target: LuaObject
    Target: LuaObject

    ctor: function
    dtor: function

    IsAlive: function(AbstractBindingType, LuaObject): boolean
    OnDataContextChanged: function(AbstractBindingType)
end

global record BindingDescriptionType
    record get
        targetName: string
        TargetName: function
    
        updateTrigger: string
        UpdateTrigger: function
    
        mode: number
        Mode: function
    
        source: SourceDescriptionType
        Source: function
    
        commandParameter: LuaObject
        CommandParameter: function
    end
    
    record set
        targetName: string
        TargetName: function
    
        updateTrigger: string
        UpdateTrigger: function
    
        mode: number
        Mode: function
    
        source: SourceDescriptionType
        Source: function
    
        commandParameter: LuaObject
        CommandParameter: function
    end

    targetName: string
    TargetName: string

    updateTrigger: string
    UpdateTrigger: string

    mode: number
    Mode: number

    source: SourceDescriptionType
    Source: SourceDescriptionType

    commandParameter: LuaObject
    CommandParameter: function

    new: function(...: any): BindingDescriptionType
    ctor: function
    dtor: function
end

global record BindingExtensionType
    record get
        bindingContext: BindingContextType
        BindingContext: function

        behaviour: LuaObject
    end

    bindingContext: BindingContextType
    BindingContext: BindingContextType

    behaviour: LuaObject

    new: function(...: any): BindingExtensionType
    ctor: function
    dtor: function

    CreateBindingSet: function(BindingExtensionType): BindingSetType
    CreateBindingSetWithContext: function(BindingExtensionType, LuaObject): BindingSetType
    SetDataContext: function(BindingExtensionType, LuaObject)
    GetDataContext: function(BindingExtensionType): LuaObject
end

global record BindingType
    record get
        bindingMode: number
        BindingMode: function
    
        targetProxy: TargetProxyBaseType
        bindingDescription: BindingDescriptionType
    end
    
    bindingMode: number
    BindingMode: number

    dataContext: LuaObject
    DataContext: LuaObject

    target: LuaObject
    Target: LuaObject

    targetProxy: TargetProxyBaseType
    sourceProxy: SourceProxyBaseType
    bindingDescription: BindingDescriptionType
    sourceProxyFactory: ISourceProxyFactoryType
    targetProxyFactory: ITargetProxyFactoryType

    new: function(...: any): BindingType
    ctor: function
    dtor: function

    CreateTargetProxy: function(BindingType, LuaObject, BindingDescriptionType)
    IsSubscribeTargetValueChanged: function(BindingType, number): boolean
    DisposeTargetProxy: function(BindingType)
    CreateSourceProxy: function(BindingType, LuaObject, SourceDescriptionType)
    IsSubscribeSourceValueChanged: function(BindingType, number): boolean
    DisposeSourceProxy: function(BindingType)
    UpdateTargetFromSource: function(BindingType)
    SetTargetValue: function(BindingType, LuaObject, any)
    UpdateSourceFromTarget: function(BindingType)
    SetSourceValue: function(BindingType, LuaObject, any)
    UpdateDataOnBind: function(BindingType)
    UpdateTargetOnFirstBind: function(BindingType, number)
    UpdateSourceOnFirstBind: function(BindingType, number)
end

global record INotifyPropertyChangedType
    record set
        propertyChanged: {EventHandlerType}
        PropertyChanged: function
    end

    propertyChanged: {EventHandlerType}
    PropertyChanged: EventHandlerType

    ctor: function
end

global record PropertyChangedEventArgsType
    record get
        propertyName: string
        PropertyName: function
    end
    
    propertyName: string
    PropertyName: string

    new: function(...: any): PropertyChangedEventArgsType
    ctor: function
end

global record StandardBinderType
    sourceProxyFactory: SourceProxyBaseType
    targetProxyFactory: TargetProxyBaseType

    Instance: StandardBinderType

    __singleton: function(StandardBinderType): StandardBinderType

    new: function(...: any): StandardBinderType
    ctor: function
    delete: function
    dtor: function

    Bind: function(StandardBinderType, LuaObject, LuaObject, BindingDescriptionType): BindingType
end

global record BindingBuilderBaseType
    context: BindingContextType
    target: LuaObject
    builded: boolean
    scopeKey: string
    description: BindingDescriptionType

    ctor: function
    dtor: function

    SetMode: function(BindingBuilderBaseType, number)
    SetMemberPath: function(BindingBuilderBaseType, string)
    SetMemberPathWithPath: function(BindingBuilderBaseType, {string})
    SetCommandParameter: function(BindingBuilderBaseType, ...: any)
    CheckBindingDescription: function(BindingBuilderBaseType)
    Build: function(BindingBuilderBaseType)
end

global record BindingBuilderType
    target: LuaObject
    description: BindingDescriptionType

    new: function(...: any): BindingBuilderType
    ctor: function
    dtor: function

    For: function(BindingBuilderType, string, string): BindingBuilderType
    To: function(BindingBuilderType, string): BindingBuilderType
    OneWay: function(BindingBuilderType): BindingBuilderType
    OneWayToSource: function(BindingBuilderType): BindingBuilderType
    TwoWay: function(BindingBuilderType): BindingBuilderType
    OneTime: function(BindingBuilderType): BindingBuilderType
    CommandParameter: function(BindingBuilderType, ...: any): BindingBuilderType
    SetMode: function(BindingBuilderType, number)
    SetMemberPath: function(BindingBuilderType, string)
    SetCommandParameter: function(BindingBuilderType, ...: any)
end

global record BindingSetBaseType
    context: BindingContextType
    builders: {BindingBuilderBaseType}

    ctor: function
    dtor: function

    Build: function(BindingSetBaseType)
end

global record BindingSetType
    builders: {BindingBuilderBaseType}
    context: BindingContextType
    target: LuaObject

    new: function(...: any): BindingSetType
    ctor: function
    dtor: function

    Bind: function(BindingSetType): BindingBuilderBaseType
    BindByTarget: function(BindingSetType, LuaObject): BindingBuilderBaseType

end

global record BindingContextType
    record get
        dataContext: LuaObject
        DataContext: function
    end
    
    record set
        dataContext: LuaObject
        DataContext: function
    
        dataContextChanged: {EventHandlerType}
        DataContextChanged: function
    
        OnDataContextChanged: function
        RaiseDataContextChanged: function
    end
    
    dataContext: LuaObject
    DataContext: LuaObject

    dataContextChanged: {EventHandlerType}
    DataContextChanged: EventHandlerType
    
    owner: LuaObject
    bindings: {string:{BindingType}}
    DEFAULT_KEY: string

    new: function(...: any): BindingContextType
    ctor: function
    dtor: function
    OnDataContextChanged: function(BindingContextType)
    RaiseDataContextChanged: function(BindingContextType)
    GetOrCreateList: function(BindingContextType, string): {BindingType}
    Add: function(BindingContextType, BindingType, string)
    AddWithSingleDesc: function(BindingContextType, LuaObject, BindingDescriptionType, string)
end

global record BindingProxyBaseType
    ctor: function
    dtor: function
end

global record IModifiableType
    SetValue: function(IModifiableType, any)
end

global record INotifiableType
    record set
        valueChanged: EventHandlerType
        ValueChanged: function
    end

    valueChanged: {EventHandlerType}
    ValueChanged: EventHandlerType
end

global record IObtainableType
    GetValue: function(IObtainableType): any
end

global record OutProxyType
    ref: LuaObject
end

global record ISourceProxyFactoryType
    CreateProxy: function(ISourceProxyFactoryType, LuaObject, SourceDescriptionType): SourceProxyBaseType
end

global record NotifiableSourceProxyBaseType
    record set
        valueChanged: {EventHandlerType}
        ValueChanged: function
    end

    valueChanged: {EventHandlerType}
    ValueChanged: function

    ctor: function
    dtor: function
    RaiseValueChanged: function(NotifiableSourceProxyBaseType)
end

global record SourceDescriptionType
    record get
        isStatic: boolean
        IsStatic: function
    end
    
    record set
        isStatic: boolean
        IsStatic: function
    end
    

    isStatic: boolean
    IsStatic: function(SourceDescriptionType): boolean

    ctor: function(SourceDescriptionType)
    dtor: function(SourceDescriptionType)
end

global record SourceProxyBaseType
    record get
        source: LuaObject
        Source: function
    end
    
    record set
        source: LuaObject
        Source: function
    end

    source: LuaObject

    ctor: function
    delete: function
    dtor: function
end

global type PriorityFactoryPairList = {PriorityFactoryPairType}

global record SourceProxyFactoryType
    factories: PriorityFactoryPairList

    new: function(...: any): SourceProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(SourceProxyFactoryType, LuaObject, SourceDescriptionType): SourceProxyBaseType
    TryCreateProxy: function(SourceProxyFactoryType, LuaObject, SourceDescriptionType, OutProxyType): boolean
    Register: function(SourceProxyFactoryType, ISourceProxyFactoryType, number)
    Unregister: function(SourceProxyFactoryType, ISourceProxyFactoryType)
end

global record TypedSourceProxyFactoryType
    ctor: function
    dtor: function

    IsSupported: function(TypedSourceProxyFactoryType, SourceDescriptionType): boolean
    CreateProxy: function(TypedSourceProxyFactoryType, LuaObject, SourceDescriptionType): SourceProxyBaseType
    TryCreateProxy: function(TypedSourceProxyFactoryType, LuaObject, SourceDescriptionType, OutProxyType): boolean
end

global record INodeProxyFactoryType
    Create: function(INodeProxyFactoryType, LuaObject, {string}): SourceProxyBaseType
end

global record InteractionNodeProxyType
    source: LuaObject
    request: string
    invoker: LuaObject

    new: function(...: any): InteractionNodeProxyType
    ctor: function
    dtor: function

    BindEvent: function(InteractionNodeProxyType)
    UnbindEvent: function(InteractionNodeProxyType)
    OnRaised: function(InteractionNodeProxyType, LuaObject, ...: any)
    SetValue: function(InteractionNodeProxyType, any)
end

global record MethodNodeProxyType
    source: LuaObject
    methodInfo: string
    invoker: ProxyInvokerType

    new: function(...: any): MethodNodeProxyType
    ctor: function
    dtor: function

    GetValue: function(MethodNodeProxyType): any
end

global record ObjectSourceDescriptionType
    record get
        path: {string}
        Path: function
    end
    
    record set
        path: {string}
        Path: function
    end

    path: {string}
    Path: {string}
    isStatic: boolean

    new: function(...: any): ObjectSourceDescriptionType
    ctor: function
    dtor: function
end

global record ObjectSourceProxyFactoryType
    factories: {PriorityFactoryPairType}

    new: function(...: any): ObjectSourceProxyFactoryType
    ctor: function
    dtor: function

    Create: function(ObjectSourceProxyFactoryType, LuaObject, {string}): SourceProxyBaseType
    TryCreateProxy: function(ObjectSourceProxyFactoryType, LuaObject, SourceDescriptionType, OutProxyType): boolean
    Register: function(ObjectSourceProxyFactoryType, INodeProxyFactoryType, number)
    Unregister: function(ObjectSourceProxyFactoryType, INodeProxyFactoryType)
end

global record PropertyNodeProxyType
    source: LuaObject
    propertyInfo: string
    
    new: function(...: any): PropertyNodeProxyType
    ctor: function
    dtor: function

    OnPropertyChanged: function(PropertyNodeProxyType, LuaObject, PropertyChangedEventArgsType)
    RaiseValueChanged: function(PropertyNodeProxyType)
    GetValue: function(PropertyNodeProxyType): any
    SetValue: function(PropertyNodeProxyType, any)
end

global record UniversalNodeProxyFactoryType
    new: function(...: any): UniversalNodeProxyFactoryType
    ctor: function
    dtor: function
    
    Create: function(UniversalNodeProxyFactoryType, LuaObject, {string}): SourceProxyBaseType
    CreateProxy: function(UniversalNodeProxyFactoryType, LuaObject, string): SourceProxyBaseType
end

global record ITargetProxyFactoryType
    CreateProxy: function(ITargetProxyFactoryType, LuaObject, BindingDescriptionType): TargetProxyBaseType
end

global record TargetProxyBaseType
    record get
        target: LuaObject
        Target: function
    
        DefaultMode: function
    end
    
    record set
        target: LuaObject
        Target: function
    end

    target: LuaObject
    Target: LuaObject

    DefaultMode: number

    targetName: string

    ctor: function
    delete: function
    dtor: function

    IsAlive: function(TargetProxyBaseType, LuaObject): boolean
end

global record TargetProxyFactoryType
    factories: PriorityFactoryPairList

    new: function(...: any): TargetProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(TargetProxyFactoryType, LuaObject, BindingDescriptionType): TargetProxyBaseType
    TryCreateProxy: function(TargetProxyFactoryType, LuaObject, BindingDescriptionType, OutProxyType): boolean
    Register: function(TargetProxyFactoryType, ITargetProxyFactoryType, number)
    Unregister: function(TargetProxyFactoryType, ITargetProxyFactoryType)
end

global record ValueTargetProxyBaseType
    record set
        subScribed: boolean
    
        valueChanged: {EventHandlerType}
        ValueChanged: function
    
        Subscribe: function(set)
    end

    subScribed: boolean

    valueChanged: {EventHandlerType}
    ValueChanged: function

    target: LuaObject

    ctor: function
    dtor: function

    Subscribe: function(ValueTargetProxyBaseType)
    Unsubscribe: function(ValueTargetProxyBaseType)
    DoSubscribeForValueChange: function(ValueTargetProxyBaseType, LuaObject)
    DoUnsubscribeForValueChange: function(ValueTargetProxyBaseType, LuaObject)
    RaiseValueChanged: function(ValueTargetProxyBaseType)
    GetValue: function(ValueTargetProxyBaseType): any
    SetValue: function(ValueTargetProxyBaseType, any)
end

global record UnityEventProxyBaseType
    record get
        DefaultMode: function
    end

    DefaultMode: number

    target: LuaObject
    unityEvent: string
    interactable: string
    command: SimpleCommandType
    invoker: ProxyInvokerType

    ctor: function
    dtor: function

    BindEvent: function(UnityEventProxyBaseType)
    UnbindEvent: function(UnityEventProxyBaseType)
    IsValidInvoker: function(UnityEventProxyBaseType, ProxyInvokerType)
    SetValue: function(UnityEventProxyBaseType, any)
    OnExecuteChanged: function(UnityEventProxyBaseType, LuaObject, ...: any)
    UpdateTargetInteractable: function(UnityEventProxyBaseType)
    BindCommand: function(UnityEventProxyBaseType, SimpleCommandType)
    UnbindCommand: function(UnityEventProxyBaseType, SimpleCommandType)
end

global record UnityEventProxyType
    target: LuaObject
    unityEvent: string

    invoker: ProxyInvokerType
    command: SimpleCommandType

    new: function(...: any): UnityEventProxyType
    ctor: function
    dtor: function

    BindEvent: function(UnityEventProxyType)
    UnbindEvent: function(UnityEventProxyType)
    IsValidInvoker: function(UnityEventProxyType, ProxyInvokerType)
    OnEvent: function(UnityEventProxyType, ...: any)
end

global record UnityPropertyProxyType
    record get
        DefaultMode: function
    end

    DefaultMode: number

    target: LuaObject
    unityEvent: string

    new: function(...: any): UnityPropertyProxyType
    ctor: function
    dtor: function

    DoSubscribeForValueChange: function(UnityPropertyProxyType, LuaObject)
    DoUnsubscribeForValueChange: function(UnityPropertyProxyType)
    OnValueChanged: function(UnityPropertyProxyType, ...: any)
    RaiseValueChanged: function(UnityPropertyProxyType, ...: any)
end

global record UnityTargetProxyFactoryType
    new: function(...: any): UnityTargetProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(UnityTargetProxyFactoryType, LuaObject, BindingDescriptionType): TargetProxyBaseType
end

global record EventTargetProxyBaseType
    ctor: function
    dtor: function

    SetValue: function(EventTargetProxyBaseType, any)
end

global record InteractionTargetProxyType

end

global record MethodTargetProxyType
    record get
        DefaultMode: function
    
        methodInfo: string
        ProxyMethodInfo: function
    end

    DefaultMode: number

    methodInfo: string
    ProxyMethodInfo: string

    target: LuaObject
    invoker: ProxyInvokerType
    
    new: function(...: any): MethodTargetProxyType
    ctor: function
    dtor: function
    GetValue: function(MethodTargetProxyType): any
    Invoke: function(MethodTargetProxyType, LuaObject, ...:any)
end

global record PropertyTargetProxyType
    record get
        DefaultMode: function
    end

    target: LuaObject
    propertyInfo: string
    
    new: function(...: any): PropertyTargetProxyType
    ctor: function
    dtor: function

    DoSubscribeForValueChange: function(PropertyTargetProxyType, LuaObject)
    DoUnsubscribeForValueChange: function(PropertyTargetProxyType, LuaObject)
    OnPropertyChanged: function(PropertyTargetProxyType, LuaObject, PropertyChangedEventArgsType)
    RaiseValueChanged: function(PropertyTargetProxyType)
    GetValue: function(PropertyTargetProxyType): any
    SetValue: function(PropertyTargetProxyType, any)
end

global record UniversalTargetProxyFactoryType
    new: function(...: any): UniversalTargetProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(UniversalTargetProxyFactoryType, LuaObject, BindingDescriptionType): TargetProxyBaseType
end

global record ProxyInvokerType
    record get
        proxyMethodInfo: string
        ProxyMethodInfo: function(get): string
    end

    target: LuaObject
    proxyMethodInfo: string

    new: function(...: any): ProxyInvokerType
    ctor: function
    dtor: function
    Invoke: function(ProxyInvokerType, ...: any): any...
    ProxyMethodInfo: function(ProxyInvokerType): string
end

global record CommandBaseType
    record set
        executeChanged: {EventHandlerType}
        ExecuteChanged: function
    end

    executeChanged: {EventHandlerType}
    ExecuteChanged: EventHandlerType

    ctor: function
    dtor: function
    RaiseExecuteChanged: function(CommandBaseType)
    CanExecute: function(CommandBaseType, ...: any)
    Execute: function(CommandBaseType, ...: any)
end

global record SimpleCommandType
    record get
        enabled: boolean
        Enabled: function
    end
    
    record set
        enabled: boolean
        Enabled: function
    
        executeChanged: {EventHandlerType}
        ExecuteChanged: function
    
        RaiseCanExecuteChanged: function(set)
    end

    enabled: boolean
    Enabled: boolean

    executeChanged: {EventHandlerType}
    ExecuteChanged: EventHandlerType

    execute: function

    new: function(...: any): SimpleCommandType
    ctor: function
    dtor: function
    RaiseExecuteChanged: function(SimpleCommandType)
    CanExecute: function(SimpleCommandType, ...: any)
    Execute: function(SimpleCommandType, ...: any)
end

global record InteractionEventArgsType
    record get
        context: LuaObject
        Context: function
    
        callback: function
        Callback: function
    end

    context: LuaObject
    Context: function
    
    callback: function
    Callback: function

    new: function(...: any): InteractionEventArgsType
    ctor: function
    dtor: function
end

global record InteractionRequestType
    record get
        raised: function
        Raised: function
    end
    
    record set
        raised: function
        Raised: function
    end
    
    raised: function
    Raised: function

    sender: LuaObject

    new: function(...: any): InteractionRequestType
    ctor: function
    dtor: function

    Raise: function(InteractionRequestType, LuaObject, function)
end

global record MessengerType
    new: function(...: any): MessengerType
end

global record ObservableObjectType
    record set
        propertyChanged: {EventHandlerType}
        PropertyChanged: function
    end

    propertyChanged: {EventHandlerType}
    PropertyChanged: EventHandlerType

    ctor: function
    dtor: function

    RaisePropertyChanged: function(ObservableObjectType, string)
    RaisePropertyChangedWithParam: function(ObservableObjectType, PropertyChangedEventArgsType)
    Set: function(ObservableObjectType, SetType, any, string, boolean)
end

global record LayerType
    record get
        windowType: number
        WindowType: function
    end

    record set
        windowType: number
        WindowType: function
    end
    
    windowType: number
    WindowType: number

    orderInLayer: number
    orderStep: number
    maxOrder: number
    minOrder: number

    new: function(...: any): LayerType
    ctor: function
    dtor: function

    PushWindowOrder: function(LayerType): number
    PopWindowOder: function(LayerType)
end

global record TransitionAwaiterType
    metamethod __call: function(TransitionAwaiterType, ...: any): TransitionAwaiterType
end

global record TransitionExecutorType
    record get
        running: boolean
        IsRunning: function
        Count: function
        transitions: {TransitionType}
    end

    taskResult: CS.CSObject.Asynchronous.AsyncResult
    running: boolean
    transitions: {TransitionType}

    new: function(...: any): TransitionExecutorType
    ctor: function
    dtor: function

    Execute: function(TransitionExecutorType, TransitionType)
    ShutDone: function(TransitionExecutorType)
    DoTask: function(TransitionExecutorType)
end

global record TransitionType
    record get
        window: WindowViewBaseType
        Window: function
    
        done: boolean
        IsDone: function
    
        animationDisabled: boolean
        AnimationDisabled: function
    
        overlayPolicy: function
        OverlayPolicy: function
    end
    
    record set
        window: WindowViewBaseType
        Window: function

        done: boolean
        IsDone: function
    
        animationDisabled: boolean
        AnimationDisabled: function
    
        overlayPolicy: function
        OverlayPolicy: function
    end
    
    window: WindowViewBaseType
    done: boolean
    animationDisabled: boolean
    overlayPolicy: function
    running: boolean
    bound: boolean
    onStart: function
    onStateChanged: function
    onFinish: function

    ctor: function
    dtor: function

    Bind: function(TransitionType)
    Unbind: function(TransitionType)
    WaitForDone: function(TransitionType): CS.System.Object
    StateChanged: function(TransitionType, WindowViewBaseType, any)
    RaiseStateChanged: function(TransitionType, WindowViewBaseType, number)
    RaiseStart: function(TransitionType)
    RaiseFinished: function(TransitionType)
    OnStart: function(TransitionType)
    OnEnd: function(TransitionType)
    GetAwaiter: function(TransitionType)
    OnStartByCallBack: function(TransitionType, function)
    OnStateChanged: function(TransitionType, function): TransitionType
    OnFinish: function(TransitionType, function)
    DisableAnimation: function(TransitionType, boolean): TransitionType
    TransitionTask: function(TransitionType, boolean)
    DoTransition: function(TransitionType)
end

global record ViewModelBaseType
    record get
        parent: CS.UnityEngine.Transform
        Parent: function
    
        localRotation: CS.UnityEngine.Quaternion
        LocalRotation: function
    
        localPosition: CS.UnityEngine.Vector3
        LocalPosition: function
    
        localScale: CS.UnityEngine.Vector3
        LocalScale: function
    
        offsetMax: CS.UnityEngine.Vector2
        OffsetMax: function
    
        offsetMin: CS.UnityEngine.Vector2
        OffsetMin: function
    
        activeSelf: boolean
        ActiveSelf: function
    
        overrideSorting: boolean
        OverrideSorting: function
    
        sortingOrder: number
        SortingOrder: function
    
        alpha: number
        Alpha: function

        Set: function
    end
    
    record set
        parent: CS.UnityEngine.Transform
        Parent: function
    
        localRotation: CS.UnityEngine.Quaternion
        LocalRotation: function
    
        localPosition: CS.UnityEngine.Vector3
        LocalPosition: function
        
        localScale: CS.UnityEngine.Vector3
        LocalScale: function
    
        offsetMax: CS.UnityEngine.Vector2
        OffsetMax: function
    
        offsetMin: CS.UnityEngine.Vector2
        OffsetMin: function
    
        activeSelf: boolean
        ActiveSelf: function
    
        overrideSorting: boolean
        OverrideSorting: function
    
        sortingOrder: number
        SortingOrder: function
    
        alpha: number
        Alpha: function

        Set: function
    end

    parent: CS.UnityEngine.Transform
    Parent: CS.UnityEngine.Transform

    localRotation: CS.UnityEngine.Quaternion
    localPosition: CS.UnityEngine.Vector3
    localScale: CS.UnityEngine.Vector3
    offsetMax: CS.UnityEngine.Vector2
    offsetMin: CS.UnityEngine.Vector2

    activeSelf: boolean
    ActiveSelf: boolean

    overrideSorting: boolean
    sortingOrder: number
    SortingOrder: number
    alpha: number

    messenger: MessengerType

    ctor: function
    dtor: function

    SetWithBroadCast: function(ViewModelBaseType, SetType, any, string, boolean)
    Broadcast: function(ViewModelBaseType, any, any, string)
    Set: function(ViewModelBaseType, SetType, any, string, boolean)
end

global record WindowBaseType
    record get
        viewStack: {WindowViewBaseType}
        Current: function
    end

    Current: WindowViewBaseType

    subView: {WindowViewBaseType}
    viewStack: {WindowViewBaseType}
    windowType: number

    ctor: function
    dtor: function

    OnCreate: function(WindowBaseType)

    OnEnable: function(WindowBaseType)
    OnDisable: function(WindowBaseType)

    Show: function(WindowBaseType, boolean)
    OnShow: function(WindowBaseType)
    
    Hide: function(WindowBaseType, boolean)
    OnHide: function(WindowBaseType)

    Dismiss: function(WindowBaseType, boolean)
    OnDismiss: function(WindowBaseType)

    AddSubView: function(WindowBaseType, WindowViewBaseType)
    PushViewStack: function(WindowBaseType, WindowViewBaseType)
    PopViewStack: function(WindowBaseType): WindowViewBaseType
end

global record WindowStateEventArgsType
    record get
        window: WindowViewBaseType
        Window: function
    
        state: number
        State: function
    
        oldState: number
        OldState: function
    end

    window: WindowViewBaseType
    Window: WindowViewBaseType

    state: number
    State: number

    oldState: number
    OldState: number
    
    new: function(...: any): WindowStateEventArgsType
    ctor: function
    dtor: function
end

global record WindowViewBaseType
    record get
        messenger: MessengerType
        Messenger: function
        
        activatedChanged: {EventHandlerType}
        ActivatedChanged: function
    
        visibilityChanged: {EventHandlerType}
        VisibilityChanged: function
    
        onDismissed: {EventHandlerType}
        OnDismissed: function
    
        stateChanged: {EventHandlerType}
        StateChanged: function
    
        created: boolean
        Created: function
    
        dismissed: boolean
        Dismissed: function
    
        activated: boolean
        Activated: function
    
        state: number
        State: function
    
        windowType: number
        WindowType: function
    
        activationAnimation: UIAnimationType
        ActivationAnimation: function

        passivationAnimation: UIAnimationType
        PassivationAnimation: function

        enterAnimation: UIAnimationType
        EnterAnimation: function
    
        exitAnimation: UIAnimationType
        ExitAnimation: function
    
        Name: function
    
        parent: CS.UnityEngine.Transform
        Parent: function
    
        gameObject: GameObjectExtType
        Owner: function
    
        transform: TransformExtType
        Transform: function
    
        Visibility: function

        Mutex: boolean

        viewModel: ViewModelBaseType
    end
    
    record set
        activatedChanged: {EventHandlerType}
        ActivatedChanged: function
    
        visibilityChanged: {EventHandlerType}
        VisibilityChanged: function
    
        onDismissed: {EventHandlerType}
        OnDismissed: function
    
        stateChanged: {EventHandlerType}
        StateChanged: function
    
        activated: boolean
        Activated: function
    
        state: number
        State: function
    
        activationAnimation: UIAnimationType
        ActivationAnimation: function

        passivationAnimation: UIAnimationType
        PassivationAnimation: function

        enterAnimation: UIAnimationType
        EnterAnimation: function
    
        exitAnimation: UIAnimationType
        ExitAnimation: function
    
        Visibility: function

        viewModel: ViewModelBaseType

        RaiseActivatedChanged: function(set)
        RaiseStateChanged: function(set, number, number)
    end

    Name: string
    Visibility: boolean
    PrefabPath: string

    activationAnimation: UIAnimationType
    ActivationAnimation: UIAnimationType

    passivationAnimation: UIAnimationType
    PassivationAnimation: UIAnimationType

    bundle: any

    messenger: MessengerType

    gameObject: GameObjectExtType
    transform: TransformExtType

    dismissTransition: TransitionType
    DismissTransition: TransitionType
    enterAnimation: UIAnimationType
    EnterAnimation: UIAnimationType
    exitAnimation: UIAnimationType
    ExitAnimation: UIAnimationType

    mutex: boolean
    Mutex: boolean

    created: boolean
    dismissed: boolean
    activated: boolean
    Activated: boolean
    
    stateBroadcast: boolean
    windowType: number
    WindowType: number
    state: number
    State: number
    
    activatedChanged: {EventHandlerType}
    ActivatedChanged: EventHandlerType
    visibilityChanged: {EventHandlerType}
    VisibilityChanged: EventHandlerType
    onDismissed: {EventHandlerType}
    OnDismissed: EventHandlerType
    stateChanged: {EventHandlerType}
    StateChanged: EventHandlerType

    viewModel: ViewModelBaseType
    bindingExt: BindingExtensionType
    bindingSet: BindingSetBaseType

    parent: CS.UnityEngine.Transform
    variables: MonoVariableArrayExtType
    components: {ComponentExtType}

    new: function(...: any): WindowViewBaseType
    ctor: function
    delete: function
    dtor: function

    OnEnable: function(WindowViewBaseType)
    OnDisable: function(WindowViewBaseType)
    RaiseActivatedChanged: function(WindowViewBaseType)
    RaiseVisibilityChanged: function(WindowViewBaseType)
    RaiseOnDismissed: function(WindowViewBaseType)
    RaiseStateChanged: function(WindowViewBaseType, number, number)
    Activate: function(WindowViewBaseType, boolean): AsyncResultExtType
    Passivate: function(WindowViewBaseType, boolean): AsyncResultExtType
    Create: function(WindowViewBaseType)
    OnCreate: function(WindowViewBaseType)
    
    Show: function(WindowViewBaseType, boolean)
    DoShow: function(WindowViewBaseType, boolean): AsyncResultExtType
    OnShow: function(WindowViewBaseType)
    
    Hide: function(WindowViewBaseType, boolean)
    DoHide: function(WindowViewBaseType, boolean): AsyncResultExtType
    OnHide: function(WindowViewBaseType)
    
    Dismiss: function(WindowViewBaseType, boolean)
    DoDismiss: function(WindowViewBaseType)
    OnDismiss: function(WindowViewBaseType)

    GetComponentWithName: function(WindowViewBaseType, ComponentExtType, string): ComponentExtType
    GetComponent: function(WindowViewBaseType, ComponentExtType): ComponentExtType
    AddComponent: function(WindowViewBaseType, ComponentExtType): ComponentExtType
end

global record UIAnimationType
    OnStart: function(UIAnimationType, function): UIAnimationType
    OnEnd: function(UIAnimationType, function): UIAnimationType
    Play: function(UIAnimationType): UIAnimationType
end

global record ButtonExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    OnClick: string

    ctor: function
    dtor: function
end

global record CanvasGroupExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    Alpha: string

    ctor: function
    dtor: function
end

global record CanvasExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    OverrideSorting: string
    SortingOrder: string

    ctor: function
    dtor: function
end

global record ComponentExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component

    new: function(...: any): ComponentExtType
    ctor: function
    delete: function
    dtor: function
end

global record GameObjectExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    ActiveSelf: string
    name: string

    new: function(...: any): GameObjectExtType
    ctor: function
    dtor: function

    GetComponent: function(GameObjectExtType, any): CS.UnityEngine.Component
    AddComponent: function(GameObjectExtType, any): CS.UnityEngine.Component
end

global record GraphicRaycasterExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component

    ctor: function
    dtor: function
end

global record MonoVariableArrayExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component

    ctor: function
    dtor: function

    Get: function(MonoVariableArrayExtType, string): CS.UnityEngine.Component
end

global record RectTransformExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    OffsetMax: string
    OffsetMin: string

    new: function(...: any): RectTransformExtType
    ctor: function
    dtor: function
end

global record TextExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    Text: string

    ctor: function
    dtor: function
end

global record TransformExtType
    handler: CS.UnityEngine.Component

    CSObject: CS.UnityEngine.Component
    LocalRotation: string
    LocalPosition: string
    LocalScale: string
    Parent: string

    ctor: function
    dtor: function
end

global record AsyncResultExtType
    record get
        csharp: CS.CSObject.Asynchronous.AsyncResult
    
        IsDone: function
        Result: function
        IsCancellationRequested: function
        IsCancelled: function
    end

    csharp: CS.CSObject.Asynchronous.AsyncResult

    new: function(...: any): AsyncResultExtType
    ctor: function
    dtor: function

    SetException: function(AsyncResultExtType, string)
    SetResult: function(AsyncResultExtType, CS.System.Object)
    SetCancelled: function(AsyncResultExtType)
    Cancel: function(AsyncResultExtType): boolean
    Callbackable: function(AsyncResultExtType): CS.System.Object
    Synchronized: function(AsyncResultExtType): CS.System.Object
    WaitForDone: function(AsyncResultExtType): CS.System.Object
end

-----------------------------------------------------------------------
global record WindowManagerType
    record get
        windowStack: {WindowViewBaseType}
        Current: function
    end

    record TransitionHideType
        animationDisabled: boolean
        AnimationDisabled: boolean
        
        window: WindowViewBaseType
        dismiss: boolean
        manager: WindowManagerType
        
        new: function(...: any): TransitionHideType
        ctor: function
        dtor: function
    
        DoTransition: function(TransitionHideType)
    end

    record TransitionShowType
        animationDisabled: boolean
        AnimationDisabled: boolean
    
        overlayPolicy: function
        OverlayPolicy: function
    
        window: WindowViewBaseType
        dismiss: boolean
        manager: WindowManagerType
        
        new: function(...: any): TransitionShowType
        ctor: function
        dtor: function
    
        Overlay: function(TransitionShowType, WindowViewBaseType): number
        DoTransition: function(TransitionShowType)
    end

    current: WindowViewBaseType
    Current: WindowViewBaseType

    layers: {LayerType}
    windowViews: {WindowViewBaseType}
    windowStack: {WindowViewBaseType}
    hideWindows: {WindowViewBaseType}

    Instance: WindowManagerType

    __singleton: function(WindowManagerType): WindowManagerType

    new: function(...: any): WindowManagerType
    ctor: function
    dtor: function

    OnEnable: function(WindowManagerType)
    OnDisable: function(WindowManagerType)

    Show: function(WindowManagerType, WindowViewBaseType): TransitionType
    Hide: function(WindowManagerType, WindowViewBaseType): TransitionType
    Dismiss: function(WindowManagerType, WindowViewBaseType): TransitionType
    
    PushWindowStack: function(WindowManagerType, WindowViewBaseType)
    PopWindowStack: function(WindowManagerType): WindowViewBaseType
    ClearWindowStack: function(WindowManagerType, WindowViewBaseType)
    GetWindow: function(WindowManagerType, string): WindowViewBaseType
    
    CreateLayers: function(WindowManagerType)
    GetLayer: function(WindowManagerType, number): LayerType
end

global record CoroutineExtType
    
    Instance: CoroutineExtType

    __singleton: function(CoroutineExtType): CoroutineExtType
    
    new: function(...: any): CoroutineExtType
    ctor: function
    dtor: function

    GetEnumerator: function(function): function
    Yield: function(CS.System.Object)
    Wait: function(number)
end

global record DefaultUIViewLocatorType
    LoadAsync: function(string, CS.System.Type): CS.CSObject.Asynchronous.ProgressResult_1_Single
    DoLoad: function(CS.CSObject.Asynchronous.ProgressResult_1_Single, string, CS.System.Type)
end

global record ExecutorsExtType
    
    csharp: CS.CSObject.Execution.Executors

    Instance: ExecutorsExtType

    __singleton: function(ExecutorsExtType): ExecutorsExtType
    
    new: function(...: any): ExecutorsExtType
    ctor: function
    dtor: function
    WaitWhile: function(ExecutorsExtType, function): CS.System.Object
    RunOnMainThread: function(ExecutorsExtType, function)
    RunOnCoroutine: function(ExecutorsExtType, function): CS.CSObject.Asynchronous.AsyncResult
    RunOnCoroutineNoReturn: function(ExecutorsExtType, function)
end

global record UnityProxyRegisterType
    record Convert
        getter: function
        setter: function
    end

    registerProxies: {string:Convert}

    Instance: UnityProxyRegisterType

    __singleton: function(UnityProxyRegisterType): UnityProxyRegisterType

    new: function(...: any): UnityProxyRegisterType
    ctor: function
    dtor: function

    Initialize: function(UnityProxyRegisterType)
    Get: function(UnityProxyRegisterType, string): Convert
end

global record PathParseType
    Parse: function(string): {string}
end

global record StringExtensionType
    Split: function(string, string): {string}
    IsNilOrEmpty: function(string): boolean
end

global record TableExtentionType
    Foreach: function({any}, function)
    FindLastIndex: function({any}, function): number
    FindIndex: function({any}, function): number
    Find: function({any}, function): any
    RemoveObj: function({any}, function): any
    Exists: function({any}, function): boolean
    ContainsKey: function({any:any}, function): boolean
    ContainsValue: function({any:any}, function): boolean
end


------------------------------------------------------------------------
global record TestVType
    PrefabPath: string

    viewModel: TestVMType
    bindingExt: BindingExtensionType
    bindingSet: BindingSetBaseType

    new: function(...: any): TestVType
    ctor: function
    dtor: function

    Create: function(TestVType)
    OnCreate: function(TestVType)
    OnShow: function(TestVType)

    GetComponentWithName: function(TestVType, ComponentExtType, string): ComponentExtType

    TestFunc: function(TestVType, any, any)
end

global record TestVMType
    record get
        test: string
        Test: function

        testColor: CS.UnityEngine.Color
        TestColor: function

        testLocalScale: CS.UnityEngine.Vector3
        TestLocalScale: function

        testActive: boolean
        TestActive: function

        testRequest: InteractionRequestType
        TestRequest: function

        testCommand: SimpleCommandType
        TestCommand: function

        Set: function(get, SetType, any, string, boolean)
    end

    record set
        test: string
        Test: function

        testColor: CS.UnityEngine.Color
        TestColor: function

        testLocalScale: CS.UnityEngine.Vector3
        TestLocalScale: function

        testActive: boolean
        TestActive: function

        testRequest: InteractionRequestType
        TestRequest: function

        testCommand: SimpleCommandType
        TestCommand: function

        Set: function(set, SetType, any, string, boolean)
    end

    test: string
    Test: string

    testColor: CS.UnityEngine.Color
    TestColor: CS.UnityEngine.Color

    testLocalScale: CS.UnityEngine.Vector3
    TestLocalScale: CS.UnityEngine.Vector3

    testActive: boolean
    TestActive: boolean

    testRequest: InteractionRequestType
    TestRequest: InteractionRequestType

    testCommand: SimpleCommandType
    TestCommand: SimpleCommandType

    new: function(...: any): TestVMType
    ctor: function
    dtor: function
    
    OnClick: function(TestVMType)
end

global record TestSubVType
    ctor: function
    new: function(...: any): TestSubVType

    Create: function(TestVType)
end