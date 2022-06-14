----------------------------------------------------------
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
require("LuaFramework.Views.WindowEnum")
require("LuaFramework.Binding.BindingMode")


---------------------------------------------------------
global record Type
    IsAssignableFrom: function(Type, any): boolean
end


---------------------------------------------------------
global typeof = function(...: any): Type
    print(...)
end

global classget = function(cls: any, key: string): any
    print(cls, key)
end

global record AsyncResult
    done: boolean
    result: BaseType
    cancellationRequested: boolean
    cancelled: boolean
    SetResult: function(AsyncResult, BaseType)
    SetCancelled: function(AsyncResult)
    Cancel: function(AsyncResult): boolean
    Callbackable: function(AsyncResult): BaseType
    Synchronized: function(AsyncResult): BaseType
    WaitForDone: function(AsyncResult): BaseType

    metamethod __call: function(AsyncResult, ...: any): AsyncResult
end

global record Executors
    WaitWhile: function(function): BaseType
    RunOnMainThread: function(function)
    RunOnCoroutine: function(function): BaseType
    RunOnCoroutineNoReturn: function(function)
end

global record Object
    name: string
    Instantiate: function(Object): Object
end

global record ProgressResult_1_Single
    Progress: number
    Callbackable: function(ProgressResult_1_Single): Object
    UpdateProgress: function(ProgressResult_1_Single, number)
    SetResult: function(ProgressResult_1_Single, any)

    metamethod __call: function(ProgressResult_1_Single, ...: any): ProgressResult_1_Single
end

global record ResourceRequest
    isDone: boolean
    progress: number
    asset: Object
end

global record Resources
    LoadAsync: function(string, any)
end

global record UnityEvent
    AddListener: function
    RemoveAllListeners: function
end

global record WaitForSeconds
    metamethod __call: function(WaitForSeconds, ...: any): WaitForSeconds
end

global record CS
    UnityEngine: UnityEngine
end

global record UnityEngine
    Object: Object
    Canvas: Canvas
    UI: UI
    Events: Events
    Resources: Resources
    Component: Component
    Transform: Transform
    GameObject: GameObject
    CanvasGroup: CanvasGroup
    RectTransform: RectTransform
    WaitForSeconds: WaitForSeconds
end

global record Canvas
end

global record Component
    GetComponent: function(Component, Type): Component
    AddComponent: function(Component, Type): Component
end

global record Transform
end

global record GameObject
end

global record CanvasGroup
end

global record RectTransform
end

global record UI
    Text: Text
    Button: Button
    GraphicRaycaster: GraphicRaycaster
end

global record Text

end

global record Button

end

global record GraphicRaycaster

end

global record Events
    UnityEventBase: UnityEventBase
end

global record UnityEventBase

end

global record UnityEvent
    AddListener: function
    RemoveAllListeners: function
end

global record CSharpType
    Object: CS.UnityEngine.Object
    Canvas: CS.UnityEngine.Canvas
    Resources: CS.UnityEngine.Resources
    CanvasGroup: CS.UnityEngine.CanvasGroup
    RectTransform: CS.UnityEngine.RectTransform
    Transform: CS.UnityEngine.Transform
    GameObject: CS.UnityEngine.GameObject
    WaitForSeconds: CS.UnityEngine.WaitForSeconds
    
    Text: CS.UnityEngine.UI.Text
    Button: CS.UnityEngine.UI.Button
    GraphicRaycaster: CS.UnityEngine.UI.GraphicRaycaster

    Simple: Simple
    Execution: Execution
    Asynchronous: Asynchronous
end

global record Simple
    MonoVariableArray: MonoVariableArray
end

global record MonoVariableArray
    variableArray: VariableArray
end

global record VariableArray
    Get: function(VariableArray, string): Component
end

global record Execution
    Executors: Executors
end

global record Asynchronous
    AsyncResult: AsyncResult
    ["ProgressResult`1[System.Single]"]: ProgressResult_1_Single
end

global record Vector3
end

global record Vector2
end

global record CSharpTypeNameType 
    ProgressResult_1_Single: string
end

global CSharpTypeName: CSharpTypeNameType = {
    ProgressResult_1_Single = "ProgressResult`1[System.Single]"
}


----------------------------------------------------------
global record LayerDataType
    WindowType: number
    OrderInLayer: number
    OrderStep: number
end

global record PriorityFactoryPairType
    factory: BaseType
    priority: number
end

----------------------------------------------------------
global record BaseType
    is: function(any): boolean
    GetType: function(BaseType): Type
end

global record EventHandlerType
    this: BaseType
    callback: function
    args: PropertyChangedEventArgsType
end

global record Functions
    ClassGet: function(cls: BaseType, key: string): any
    ClassSet: function(cls: BaseType, key: string, value: any)
end

global record AbstractBindingType
    record get
        dataContext: BaseType
        DataContext: function
    
        target: BaseType
        Target: function
    end
    
    record set
        dataContext: BaseType
        DataContext: function
    end

    dataContext: BaseType
    DataContext: BaseType
    target: BaseType
    Target: BaseType

    ctor: function
    dtor: function

    IsAlive: function(AbstractBindingType, BaseType)
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
    
        commandParameter: BaseType
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
    
        commandParameter: BaseType
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

    commandParameter: BaseType
    CommandParameter: function

    new: function(...: any): BindingDescriptionType
    ctor: function
    dtor: function
end

global record BindingExtensionType
    record get
        bindingContext: BindingContextType
        BindingContext: function

        behaviour: BaseType
    end

    bindingContext: BindingContextType
    BindingContext: BindingContextType

    behaviour: BaseType

    new: function(...: any): BindingExtensionType
    ctor: function
    dtor: function

    CreateBindingSet: function(BindingExtensionType): BindingSetType
    CreateBindingSetWithContext: function(BindingExtensionType, BaseType): BindingSetType
    SetDataContext: function(BindingExtensionType, BaseType)
    GetDataContext: function(BindingExtensionType): BaseType
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

    dataContext: BaseType
    DataContext: BaseType

    target: BaseType
    Target: BaseType

    targetProxy: TargetProxyBaseType
    sourceProxy: SourceProxyBaseType
    bindingDescription: BindingDescriptionType
    sourceProxyFactory: ISourceProxyFactoryType
    targetProxyFactory: ITargetProxyFactoryType

    new: function(...: any): BindingType
    ctor: function
    dtor: function

    CreateTargetProxy: function(BindingType, BaseType, BindingDescriptionType)
    IsSubscribeTargetValueChanged: function(BindingType, number): boolean
    DisposeTargetProxy: function(BindingType)
    CreateSourceProxy: function(BindingType, BaseType, SourceDescriptionType)
    IsSubscribeSourceValueChanged: function(BindingType, number): boolean
    DisposeSourceProxy: function(BindingType)
    UpdateTargetFromSource: function(BindingType)
    SetTargetValue: function(BindingType, BaseType, any)
    UpdateSourceFromTarget: function(BindingType)
    SetSourceValue: function(BindingType, BaseType, any)
    UpdateDataOnBind: function(BindingType)
    UpdateTargetOnFirstBind: function(BindingType, number)
    UpdateSourceOnFirstBind: function(BindingType, number)
end

global record INotifyPropertyChangedType
    record set
        propertyChanged: EventHandlerType
        PropertyChanged: function
    end

    propertyChanged: EventHandlerType
    PropertyChanged: EventHandlerType
end

global record PropertyChangedEventArgsType
    record get
        propertyName: string
        PropertyName: function
    end
    
    
    propertyName: string
    PropertyName: string

    ctor: function
end

global record ProxyFunctionType
    getter: function
    setter: function
end

global record UnityProxyRegisterType
    registerProxies: {ProxyFunctionType}

    new: function(...: any): UnityProxyRegisterType
    ctor: function
    dtor: function

    __singleton: function(UnityProxyRegisterType)

    Initialize: function(UnityProxyRegisterType)
    Get: function(UnityProxyRegisterType, string): ProxyFunctionType
end

global record StandardBinderType
    sourceProxyFactory: SourceProxyBaseType
    targetProxyFactory: TargetProxyBaseType

    __singleton: function(StandardBinderType): StandardBinderType

    new: function(...: any): StandardBinderType
    ctor: function
    delete: function
    dtor: function

    Bind: function(StandardBinderType, BaseType, BaseType, BindingDescriptionType): BindingType
end

global record BindingBuilderBaseType
    context: BindingContextType
    target: BaseType
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
    target: BaseType
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
    target: BaseType

    new: function(...: any): BindingSetType
    ctor: function
    dtor: function

    Bind: function(BindingSetType): BindingBuilderBaseType
    BindByTarget: function(BindingSetType, BaseType): BindingBuilderBaseType
end

global record BindingContextType
    record get
        dataContext: BaseType
        DataContext: function
    end
    
    record set
        dataContext: BaseType
        DataContext: function
    
        dataContextChanged: {EventHandlerType}
        DataContextChanged: function
    
        OnDataContextChanged: function
        RaiseDataContextChanged: function
    end
    
    dataContext: BaseType
    DataContext: BaseType

    dataContextChanged: {EventHandlerType}
    DataContextChanged: EventHandlerType
    
    owner: BaseType
    bindings: {string:{BindingType}}
    DEFAULT_KEY: string

    new: function(...: any): BindingContextType
    ctor: function
    dtor: function
    OnDataContextChanged: function(BindingContextType)
    RaiseDataContextChanged: function(BindingContextType)
    GetOrCreateList: function(BindingContextType, string): {BindingType}
    Add: function(BindingContextType, BindingType, string)
    AddWithSingleDesc: function(BindingContextType, BaseType, BindingDescriptionType, string)
end

global record PathParseType
    Parse: function(string): {string}
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
    ref: BaseType
end

global record ISourceProxyFactoryType
    CreateProxy: function(ISourceProxyFactoryType, BaseType, SourceDescriptionType): SourceProxyBaseType
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
        source: BaseType
        Source: function
    end
    
    record set
        source: BaseType
        Source: function
    end

    source: BaseType

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

    CreateProxy: function(SourceProxyFactoryType, BaseType, SourceDescriptionType): SourceProxyBaseType
    TryCreateProxy: function(SourceProxyFactoryType, BaseType, SourceDescriptionType, OutProxyType): boolean
    Register: function(SourceProxyFactoryType, ISourceProxyFactoryType, number)
    Unregister: function(SourceProxyFactoryType, ISourceProxyFactoryType)
end

global record TypedSourceProxyFactoryType
    ctor: function
    dtor: function

    IsSupported: function(TypedSourceProxyFactoryType, SourceDescriptionType): boolean
    CreateProxy: function(TypedSourceProxyFactoryType, BaseType, SourceDescriptionType): SourceProxyBaseType
    TryCreateProxy: function(TypedSourceProxyFactoryType, BaseType, SourceDescriptionType, OutProxyType): boolean
end

global record INodeProxyFactoryType
    Create: function(INodeProxyFactoryType, BaseType, {string}): SourceProxyBaseType
end

global record InteractionNodeProxyType
    source: BaseType
    request: string
    invoker: BaseType

    new: function(...: any): InteractionNodeProxyType
    ctor: function
    dtor: function

    BindEvent: function(InteractionNodeProxyType)
    UnbindEvent: function(InteractionNodeProxyType)
    OnRaised: function(InteractionNodeProxyType, BaseType, ...: any)
    SetValue: function(InteractionNodeProxyType, any)
end

global record MethodNodeProxyType
    source: BaseType
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

    Create: function(ObjectSourceProxyFactoryType, BaseType, {string}): SourceProxyBaseType
    TryCreateProxy: function(ObjectSourceProxyFactoryType, BaseType, SourceDescriptionType, OutProxyType): boolean
    Register: function(ObjectSourceProxyFactoryType, INodeProxyFactoryType, number)
    Unregister: function(ObjectSourceProxyFactoryType, INodeProxyFactoryType)
end

global record PropertyNodeProxyType
    source: BaseType
    propertyInfo: string
    
    new: function(...: any): PropertyNodeProxyType
    ctor: function
    dtor: function

    OnPropertyChanged: function(PropertyNodeProxyType, BaseType, PropertyChangedEventArgsType)
    RaiseValueChanged: function(PropertyNodeProxyType)
    GetValue: function(PropertyNodeProxyType): any
    SetValue: function(PropertyNodeProxyType, any)
end

global record UniversalNodeProxyFactoryType
    new: function(...: any): UniversalNodeProxyFactoryType
    ctor: function
    dtor: function
    
    Create: function(UniversalNodeProxyFactoryType, BaseType, {string}): SourceProxyBaseType
    CreateProxy: function(UniversalNodeProxyFactoryType, BaseType, string): SourceProxyBaseType
end

global record ITargetProxyFactoryType
    CreateProxy: function(ITargetProxyFactoryType, BaseType, BindingDescriptionType): TargetProxyBaseType
end

global record TargetProxyBaseType
    record get
        target: BaseType
        Target: function
    
        DefaultMode: function
    end
    
    record set
        target: BaseType
        Target: function
    end

    target: BaseType
    Target: BaseType

    DefaultMode: number

    targetName: string

    ctor: function
    delete: function
    dtor: function

    IsAlive: function(TargetProxyBaseType, BaseType): boolean
end

global record TargetProxyFactoryType
    factories: PriorityFactoryPairList

    new: function(...: any): TargetProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(TargetProxyFactoryType, BaseType, BindingDescriptionType): TargetProxyBaseType
    TryCreateProxy: function(TargetProxyFactoryType, BaseType, BindingDescriptionType, OutProxyType): boolean
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

    target: BaseType

    ctor: function
    dtor: function

    Subscribe: function(ValueTargetProxyBaseType)
    Unsubscribe: function(ValueTargetProxyBaseType)
    DoSubscribeForValueChange: function(ValueTargetProxyBaseType, BaseType)
    DoUnsubscribeForValueChange: function(ValueTargetProxyBaseType, BaseType)
    RaiseValueChanged: function(ValueTargetProxyBaseType)
    GetValue: function(ValueTargetProxyBaseType): any
    SetValue: function(ValueTargetProxyBaseType, any)
end

global record UnityEventProxyBaseType
    record get
        DefaultMode: function
    end

    DefaultMode: number

    target: BaseType
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
    OnExecuteChanged: function(UnityEventProxyBaseType, BaseType, ...: any)
    UpdateTargetInteractable: function(UnityEventProxyBaseType)
    BindCommand: function(UnityEventProxyBaseType, SimpleCommandType)
    UnbindCommand: function(UnityEventProxyBaseType, SimpleCommandType)
end

global record UnityEventProxyType
    target: BaseType
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

    target: BaseType
    unityEvent: string

    new: function(...: any): UnityPropertyProxyType
    ctor: function
    dtor: function

    DoSubscribeForValueChange: function(UnityPropertyProxyType, BaseType)
    DoUnsubscribeForValueChange: function(UnityPropertyProxyType)
    OnValueChanged: function(UnityPropertyProxyType, ...: any)
    RaiseValueChanged: function(UnityPropertyProxyType, ...: any)
end

global record UnityTargetProxyFactoryType
    new: function(...: any): UnityTargetProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(UnityTargetProxyFactoryType, BaseType, BindingDescriptionType): TargetProxyBaseType
end

global record EventTargetProxyBaseType
    ctor: function
    dtor: function

    SetValue: function(EventTargetProxyBaseType, any)
end

global record InteractionTargetProxyType
    ctor: function
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

    target: BaseType
    invoker: ProxyInvokerType
    
    new: function(...: any): MethodTargetProxyType
    ctor: function
    dtor: function
    GetValue: function(MethodTargetProxyType): any
    Invoke: function(MethodTargetProxyType, BaseType, ...:any)
end

global record PropertyTargetProxyType
    record get
        DefaultMode: function
    end

    target: BaseType
    propertyInfo: string
    
    new: function(...: any): PropertyTargetProxyType
    ctor: function
    dtor: function

    DoSubscribeForValueChange: function(PropertyTargetProxyType, BaseType)
    DoUnsubscribeForValueChange: function(PropertyTargetProxyType, BaseType)
    OnPropertyChanged: function(PropertyTargetProxyType, BaseType, PropertyChangedEventArgsType)
    RaiseValueChanged: function(PropertyTargetProxyType)
    GetValue: function(PropertyTargetProxyType): any
    SetValue: function(PropertyTargetProxyType, any)
end

global record UniversalTargetProxyFactoryType
    new: function(...: any): UniversalTargetProxyFactoryType
    ctor: function
    dtor: function

    CreateProxy: function(UniversalTargetProxyFactoryType, BaseType, BindingDescriptionType): TargetProxyBaseType
end

global record ProxyInvokerType
    record get
        proxyMethodInfo: string
        ProxyMethodInfo: function(get): string
    end

    target: BaseType
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

global record AsyncResultExtType
    record get
        csharp: AsyncResult
    
        IsDone: function
        Result: function
        IsCancellationRequested: function
        IsCancelled: function
    end

    csharp: AsyncResult

    ctor: function
    dtor: function
    SetException: function(AsyncResultExtType, string)
    SetResult: function(AsyncResultExtType, BaseType)
    SetCancelled: function(AsyncResultExtType)
    Cancel: function(AsyncResultExtType): boolean
    Callbackable: function(AsyncResultExtType): BaseType
    Synchronized: function(AsyncResultExtType): BaseType
    WaitForDone: function(AsyncResultExtType): BaseType
end

global record CoroutineExtType
    
    __singleton: function(CoroutineExtType): CoroutineExtType
    
    new: function(...: any): CoroutineExtType
    ctor: function
    dtor: function

    GetEnumerator: function(function): function
    Yield: function(function)
    Wait: function(number)
end

global record ExecutorsExtType
    
    csharp: Executors

    __singleton: function(ExecutorsExtType): ExecutorsExtType
    
    new: function(...: any): ExecutorsExtType
    ctor: function
    dtor: function
    WaitWhile: function(ExecutorsExtType, function): BaseType
    RunOnMainThread: function(ExecutorsExtType, function)
    RunOnCoroutine: function(ExecutorsExtType, function): BaseType
    RunOnCoroutineNoReturn: function(ExecutorsExtType, function)
end

global record InteractionEventArgsType
    record get
        context: BaseType
        Context: function
    
        callback: function
        Callback: function
    end

    context: BaseType
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

    sender: BaseType

    ctor: function
    dtor: function

    Raise: function(InteractionRequestType, BaseType, function)
end

global record MessengerType

end

global record ObservableObjectType

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
    end

    taskResult: BaseType
    running: boolean
    transitions: {TransitionType}

    ctor: function
    dtor: function

    Execute: function(TransitionExecutorType, TransitionType)
    ShutDone: function(TransitionExecutorType)
    DoTask: function(TransitionExecutorType)
end

global record TransitionHideType
    dismiss: boolean
    manager: WindowManagerType
    
    ctor: function
    dtor: function

    DoTransition: function(TransitionHideType)
end

global record TransitionShowType
    dismiss: boolean
    manager: WindowManagerType
    
    ctor: function
    dtor: function

    Overlay: function(TransitionShowType, WindowBaseType)
    DoTransition: function(TransitionShowType)
end

global record TransitionType
    record get
        window: WindowBaseType
        Window: function
    
        done: boolean
        IsDone: function
    
        animationDisabled: boolean
        AnimationDisabled: function
    
        overlayPolicy: function
        OverlayPolicy: function
    end
    
    record set
        window: WindowBaseType
        Window: function

        done: boolean
        IsDone: function
    
        animationDisabled: boolean
        AnimationDisabled: function
    
        overlayPolicy: function
        OverlayPolicy: function
    end
    
    window: WindowBaseType
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
    WaitForDone: function(TransitionType)
    StateChanged: function(TransitionType, BaseType, any)
    RaiseStateChanged: function(TransitionType, WindowBaseType, number)
    RaiseStart: function(TransitionType)
    RaiseFinished: function(TransitionType)
    OnStart: function(TransitionType)
    OnEnd: function(TransitionType)
    GetAwaiter: function(TransitionType)
    OnStartByCallBack: function(TransitionType, function)
    OnStateChanged: function(TransitionType, function)
    OnFinish: function(TransitionType, function)
    DisableAnimation: function(TransitionType, boolean)
    TransitionTask: function(TransitionType, boolean)
    DoTransition: function(TransitionType)
end

global record ViewModelBaseType
    record get
        parent: TransformExtType
        Parent: function
    
        localRotation: Vector3
        LocalRotation: function
    
        localPosition: Vector3
        LocalPosition: function
    
        localScale: Vector3
        LocalScale: function
    
        offsetMax: Vector2
        OffsetMax: function
    
        offsetMin: Vector2
        OffsetMin: function
    
        activeSelf: boolean
        ActiveSelf: function
    
        overrideSorting: boolean
        OverrideSorting: function
    
        sortingOrder: number
        SortingOrder: function
    
        alpha: number
        Alpha: function
    end
    
    record set
        parent: TransformExtType
        Parent: function
    
        localRotation: Vector3
        LocalRotation: function
    
        localPosition: Vector3
        LocalPosition: function
        
        localScale: Vector3
        LocalScale: function
    
        offsetMax: Vector2
        OffsetMax: function
    
        offsetMin: Vector2
        OffsetMin: function
    
        activeSelf: boolean
        ActiveSelf: function
    
        overrideSorting: boolean
        OverrideSorting: function
    
        sortingOrder: number
        SortingOrder: function
    
        alpha: number
        Alpha: function
    end
    ctor: function
    dtor: function

    SetWithBroadCast: function(ViewModelBaseType, BaseType, any, string, boolean)
    Broadcast: function(ViewModelBaseType, any, any, string)
end

global record WindowBaseType
    record get
        viewStack: {WindowViewBaseType}
        Current: function
    end

    subView: {WindowViewBaseType}
    viewStack: {WindowViewBaseType}
    windowType: number

    ctor: function
    dtor: function

    OnEnable: function(WindowBaseType)
    OnDisable: function(WindowBaseType)
    OnShow: function(WindowBaseType)
    OnHide: function(WindowBaseType)
    CreateSubView: function(WindowBaseType, string, any)
    AddSubView: function(WindowBaseType, WindowViewBaseType)
    PushOpenView: function(WindowBaseType, WindowViewBaseType)
    PopOpenView: function(WindowBaseType)
end

global record WindowManagerType
    record get
        windowStack: {WindowViewBaseType}
        Current: function
    end

    current: WindowViewBaseType
    layers: {LayerType}
    windowViews: {WindowViewBaseType}
    windowStack: {WindowViewBaseType}
    hideWindows: {WindowViewBaseType}

    __singleton: function(WindowManagerType): WindowManagerType

    new: function(...: any): WindowManagerType
    ctor: function
    dtor: function

    Open: function(WindowManagerType, string)
    OnEnable: function(WindowManagerType)
    OnDisable: function(WindowManagerType)
    Show: function(WindowManagerType, WindowViewBaseType)
    Hide: function(WindowManagerType, WindowViewBaseType)
    Dismiss: function(WindowManagerType, WindowViewBaseType)
    PushWindowStack: function(WindowManagerType, WindowViewBaseType)
    PopWindowStack: function(WindowManagerType)
    Open: function(WindowManagerType, string, any)
    Close: function(WindowManagerType, string)
    CreateLayers: function(WindowManagerType)
    GetLayer: function(WindowManagerType, number)
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
    
        state: boolean
        State: function
    
        windowType: boolean
        WindowType: function
    
        enterAnimation: UIAnimationType
        EnterAnimation: function
    
        exitAnimation: UIAnimationType
        ExitAnimation: function
    
        Name: function
    
        parent: TransformExtType
        Parent: function
    
        gameObject: GameObjectExtType
        Owner: function
    
        transform: TransformExtType
        Transform: function
    
        Visibility: function
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
    
        state: boolean
        State: function
    
        enterAnimation: UIAnimationType
        EnterAnimation: function
    
        exitAnimation: UIAnimationType
        ExitAnimation: function
    
        Visibility: function
    end

    bundle: any

    messenger: MessengerType

    gameObject: GameObjectExtType
    transform: TransformExtType

    dismissTransition: TransitionType
    enterAnimation: UIAnimationType
    exitAnimation: UIAnimationType

    mutex: boolean
    created: boolean
    dismissed: boolean
    activated: boolean
    stateBroadcast: boolean
    windowType: number
    state: number
    
    activatedChanged: {EventHandlerType}
    visibilityChanged: {EventHandlerType}
    onDismissed: {EventHandlerType}
    stateChanged: {EventHandlerType}

    viewModel: ViewModelBaseType
    bindingExt: BindingExtensionType
    bindingSet: BindingSetType

    parent: TransformExtType
    variables: MonoVariableArrayExtType
    components: {ComponentExtType}

    ctor: function
    dtor: function

    OnEnable: function(WindowViewBaseType)
    OnDisable: function(WindowViewBaseType)
    RaiseActivatedChanged: function(WindowViewBaseType)
    RaiseVisibilityChanged: function(WindowViewBaseType)
    RaiseOnDismissed: function(WindowViewBaseType)
    RaiseStateChanged: function(WindowViewBaseType)
    Activate: function(WindowViewBaseType, boolean)
    Passivate: function(WindowViewBaseType, boolean)
    Create: function(WindowViewBaseType)
    OnCreate: function(WindowViewBaseType)
    
    Show: function(WindowViewBaseType, boolean)
    DoShow: function(WindowViewBaseType, boolean)
    OnShow: function(WindowViewBaseType)
    
    Hide: function(WindowViewBaseType, boolean)
    DoHide: function(WindowViewBaseType, boolean)
    OnHide: function(WindowViewBaseType)
    
    Dismiss: function(WindowViewBaseType, boolean)
    DoDismiss: function(WindowViewBaseType)
    OnDismiss: function(WindowViewBaseType)

    GetComponentWithName: function(WindowViewBaseType, ComponentExtType, string)
    GetComponent: function(WindowViewBaseType, ComponentExtType)
    AddComponent: function(WindowViewBaseType, ComponentExtType)
end

global record UIAnimationType

end

global record ButtonExtType
    handler: Component

    CSharpType: Component
    OnClick: string

    ctor: function
    dtor: function
end

global record CanvasGroupExtType
    handler: Component

    CSharpType: Component
    Alpha: string

    ctor: function
    dtor: function
end

global record CanvasExtType
    handler: Component

    CSharpType: Component
    OverrideSorting: string
    SortingOrder: string

    ctor: function
    dtor: function
end

global record ComponentExtType
    handler: Component

    CSharpType: Component

    ctor: function
    dtor: function
end

global record GameObjectExtType
    handler: Component

    CSharpType: Component
    ActiveSelf: string

    ctor: function
    dtor: function

    GetComponent: function(GameObjectExtType, any): Component
    AddComponent: function(GameObjectExtType, any): Component
end

global record GraphicRaycasterExtType
    handler: Component

    CSharpType: Component

    ctor: function
    dtor: function
end

global record MonoVariableArrayExtType
    handler: Component

    CSharpType: Component

    ctor: function
    dtor: function

    Get: function(MonoVariableArrayExtType, string): Component
end

global record RectTransformExtType
    handler: Component

    CSharpType: Component
    OffsetMax: string
    OffsetMin: string

    ctor: function
    dtor: function
end

global record TextExtType
    handler: Component

    CSharpType: Component
    Text: string

    ctor: function
    dtor: function
end

global record TransformExtType
    handler: Component

    CSharpType: Component
    LocalRotation: string
    LocalPosition: string
    LocalScale: string
    Parent: string

    ctor: function
    dtor: function
end

global record DefaultUIViewLocatorType

    __singleton: function(DefaultUIViewLocatorType): DefaultUIViewLocatorType

    new: function(...: any): DefaultUIViewLocatorType
    ctor: function
    dtor: function

    LoadAsync: function(DefaultUIViewLocatorType, string, any): ProgressResult_1_Single
    DoLoad: function(DefaultUIViewLocatorType, ProgressResult_1_Single, string, any)
end

-----------------------------------------------------------------------

global PathParse: PathParseType
global StringExtension: StringExtensionType
global TableExtention: TableExtentionType
global UnityProxyRegister: UnityProxyRegisterType

global ExecutorsExt: ExecutorsExtType
global CoroutineExt: CoroutineExtType
global AsyncResultExt: AsyncResultExtType
