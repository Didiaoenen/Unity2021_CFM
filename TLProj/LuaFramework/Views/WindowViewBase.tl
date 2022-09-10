local Misc = require("LuaFramework.Common.Misc")
local TableExtention = require("LuaFramework.Common.TableExtention")
local CoroutineExt = require("LuaFramework.CSharpObject.CoroutineExt")
local ExecutorsExt = require("LuaFramework.CSharpObject.ExecutorsExt")
local DefaultUIViewLocator = require("LuaFramework.Views.Locators.DefaultUIViewLocator")

local WindowEnum = require("LuaFramework.Views.WindowEnum")
local Messenger = require("LuaFramework.Messaging.Messenger")
local AsyncResultExt = require("LuaFramework.CSharpObject.AsyncResultExt")
local WindowStateEventArgs = require("LuaFramework.Views.WindowStateEventArgs")

local CanvasExt = require("LuaFramework.Views.Components.CanvasExt")
local GameObjectExt = require("LuaFramework.Views.Components.GameObjectExt")
local CanvasGroupExt = require("LuaFramework.Views.Components.CanvasGroupExt")
local RectTransformExt = require("LuaFramework.Views.Components.RectTransformExt")
local GraphicRaycasterExt = require("LuaFramework.Views.Components.GraphicRaycasterExt")
local MonoVariableArrayExt = require("LuaFramework.Views.Components.MonoVariableArrayExt")

local WindowManager = require("LuaFramework.Views.WindowManager")

local WindowViewBase = class("WindowViewBase") as WindowViewBaseType

WindowViewBase.PrefabPath = ""

function WindowViewBase.get:Messenger(): MessengerType
    return self.messenger
end

function WindowViewBase.set:ActivatedChanged(value: EventHandlerType)
    setcallback(self.activatedChanged, value)
end

function WindowViewBase.set:VisibilityChanged(value: EventHandlerType)
    setcallback(self.visibilityChanged, value)
end

function WindowViewBase.set:OnDismissed(value: EventHandlerType)
    setcallback(self.onDismissed, value)
end

function WindowViewBase.set:StateChanged(value: EventHandlerType)
    setcallback(self.stateChanged, value)
end

function WindowViewBase.get:Created(): boolean
    return self.created
end

function WindowViewBase.get:Dismissed(): boolean
    return self.dismissed
end

function WindowViewBase.get:Activated(): boolean
    return self.activated
end

function WindowViewBase.set:Activated(value: boolean)
    if value ~= self.activated then
        self.activated = value
        self:RaiseActivatedChanged()
    end
end

function WindowViewBase.get:State(): number
    return self.state
end

function WindowViewBase.set:State(value: number)
    if value ~= self.state then
        local old = self.state
        self.state = value
        self:RaiseStateChanged(old, self.state)
    end
end

function WindowViewBase.get:WindowType(): number
    return self.windowType
end

function WindowViewBase.get:EnterAnimation(): UIAnimationType
    return self.enterAnimation
end

function WindowViewBase.set:EnterAnimation(value: UIAnimationType)
    self.enterAnimation = value
end

function WindowViewBase.get:ExitAnimation(): UIAnimationType
    return self.exitAnimation
end

function WindowViewBase.set:ExitAnimation(value: UIAnimationType)
    self.exitAnimation = value
end

function WindowViewBase.get:Name(): string
    assert(self.gameObject, "")
    return self.gameObject.name
end

function WindowViewBase.get:Parent(): TransformExtType
    return self.parent
end

function WindowViewBase.get:Owner(): GameObjectExtType
    return self.gameObject
end

function WindowViewBase.get:Transform(): TransformExtType
    return self.transform
end

function WindowViewBase.get:Visibility(): boolean
    assert(self.viewModel, "")
    return self.viewModel.ActiveSelf
end

function WindowViewBase.set:Visibility(value: boolean)
    if not self.viewModel then
        return
    end

    if self.viewModel.ActiveSelf == value then
        return
    end

    self.viewModel.ActiveSelf = value
end

function WindowViewBase:ctor(bundle)
    self.bundle = bundle

    self.messenger = Messenger.new()

    self.gameObject = nil
    self.transform = nil

    self.dismissTransition = nil
    self.enterAnimation = nil
    self.exitAnimation = nil

    self.mutex = true
    self.created = false
    self.dismissed = false
    self.activated = false
    self.stateBroadcast = true
    self.windowType = WindowEnum.WindowType.NONE
    self.state = WindowEnum.WindowState.NONE
    
    self.activatedChanged = {}
    self.visibilityChanged = {}
    self.onDismissed = {}
    self.stateChanged = {}

    self.viewModel = nil
    self.bindingExt = nil
    self.bindingSet = nil

    self.parent = nil
    self.variables = nil
    self.components = {}
    self.gameObjects = {}
end

function WindowViewBase:OnEnable()
    self:RaiseVisibilityChanged()
end

function WindowViewBase:OnDisable()
    self:RaiseVisibilityChanged()
end

function WindowViewBase:RaiseActivatedChanged()
    TableExtention.Foreach(self.activatedChanged, function (_: number, v: EventHandlerType)
        v.callback(v.this, self)
    end)
end

function WindowViewBase:RaiseVisibilityChanged()
    TableExtention.Foreach(self.visibilityChanged, function (_: number, v: EventHandlerType)
        v.callback(v.this, self)
    end)
end

function WindowViewBase:RaiseOnDismissed()
    TableExtention.Foreach(self.onDismissed, function (_: number, v: EventHandlerType)
        v.callback(v.this, self)
    end)
end

function WindowViewBase:RaiseStateChanged(oldState: number, newState: number)
    local eventArgs = WindowStateEventArgs.new(self, oldState, newState)
    TableExtention.Foreach(self.stateChanged, function (_: number, v: EventHandlerType)
        v.callback(v.this, self, eventArgs)
    end)
end

function WindowViewBase:Activate(ignoreAnimation: boolean): AsyncResultExtType
    local result = AsyncResultExt.new()

    if not self.Visibility then
        return result
    end

    if self.Activated then
        result:SetResult()
        return result
    end

    if not ignoreAnimation and self.ActivationAnimation then
        self.ActivationAnimation:OnStart(function ()
            self.State = WindowEnum.WindowState.ACTIVATION_ANIMATION_BEGIN
        end):OnEnd(function ()
            self.State = WindowEnum.WindowState.ACTIVATION_ANIMATION_END
            self.Activated = true
            self.State = WindowEnum.WindowState.ACTIVATED
            result:SetResult()
        end):Play()
    else
        self.Activated = true
        self.State = WindowEnum.WindowState.ACTIVATED
        result:SetResult()
    end

    return result
end

function WindowViewBase:Passivate(ignoreAnimation: boolean): AsyncResultExtType
    local result = AsyncResultExt.new()

    if not self.Visibility then
        return result
    end

    if not self.Activated then
        result:SetResult()
        return result
    end

    self.Activated = false
    self.State = WindowEnum.WindowState.PASSIVATED

    if not ignoreAnimation and self.ActivationAnimation then
        self.ActivationAnimation:OnStart(function ()
            self.State = WindowEnum.WindowState.PASSIVATION_ANIMATION_BEGIN
        end):OnEnd(function ()
            self.State = WindowEnum.WindowState.PASSIVATION_ANIMATION_END
            result:SetResult()
        end):Play()
    else
        result:SetResult()
    end

    return result
end

function WindowViewBase:Create()
    if self.dismissTransition or self.dismissed then
        return
    end

    if self.created then
        return
    end
    
    ExecutorsExt.Instance:RunOnCoroutineNoReturn(CoroutineExt.GetEnumerator(function ()

        self.State = WindowEnum.WindowState.CREATE_BEGIN
        self.Visibility = false

        local result = DefaultUIViewLocator.LoadAsync(self.PrefabPath, Misc.CSType.GameObjectT)
        CoroutineExt.Yield(result:WaitForDone())
        print("加载完成。。。")

        self.gameObject = GameObjectExt.new(result.Result)

        self:OnCreate()
        self.created = true
        self.State = WindowEnum.WindowState.CREATE_END

        self:Show()
    end))
end

function WindowViewBase:OnCreate()
    print("parent OnCreate")
    
    self.parent = self.viewModel.parent

    self.variables = self:GetComponent(MonoVariableArrayExt) as MonoVariableArrayExtType

    local bindingSet = self.bindingSet as BindingSetType

    -- GameObject
    local builder = bindingSet:BindByTarget(self.gameObject as LuaObject)
    builder:For(self.gameObject.ActiveSelf):To("ActiveSelf"):OneWay()

    -- RectTransform
    self.transform = self:GetComponent(RectTransformExt as ComponentExtType) as TransformExtType
    builder = bindingSet:BindByTarget(self.transform as LuaObject)
    builder:For(self.transform.Parent):To("Parent"):OneWay()
    
    builder = bindingSet:BindByTarget(self.transform as LuaObject)
    builder:For(self.transform.LocalRotation):To("LocalRotation"):OneWay()
    
    builder = bindingSet:BindByTarget(self.transform as LuaObject)
    builder:For(self.transform.LocalPosition):To("LocalPosition"):OneWay()

    builder = bindingSet:BindByTarget(self.transform as LuaObject)
    builder:For(self.transform.LocalScale):To("LocalScale"):OneWay()

    builder = bindingSet:BindByTarget(self.transform as LuaObject)
    builder:For((self.transform as RectTransformExtType).OffsetMax):To("OffsetMax"):OneWay()

    builder = bindingSet:BindByTarget(self.transform as LuaObject)
    builder:For((self.transform as RectTransformExtType).OffsetMin):To("OffsetMin"):OneWay()

    -- Canvas
    local canvas = self:AddComponent(CanvasExt) as CanvasExtType
    builder = bindingSet:BindByTarget(canvas.handler as LuaObject)
    builder:For(canvas.OverrideSorting):To("OverrideSorting"):OneWay()
    
    builder = bindingSet:BindByTarget(canvas.handler as LuaObject)
    builder:For(canvas.SortingOrder):To("SortingOrder"):OneWay()

    -- CanvasGroup
    local canvasGroup = self:AddComponent(CanvasGroupExt) as CanvasGroupExtType
    builder = bindingSet:BindByTarget(canvasGroup.handler as LuaObject)
    builder:For(canvasGroup.Alpha):To("Alpha"):OneWay()

    -- GraphicRaycaster
    self:AddComponent(GraphicRaycasterExt)

    self.bindingSet:Build()

    --
    local layer = WindowManager.Instance:GetLayer(self.windowType)
    assert(layer, "")

    self.viewModel.SortingOrder = layer:PushWindowOrder()
end

function WindowViewBase:Show(ignoreAnimation: boolean): TransitionType
    if self.dismissTransition or self.dismissed then
        return
    end

    if self.Visibility then
        return
    end

    return WindowManager.Instance:Show(self):DisableAnimation(ignoreAnimation)
end

function WindowViewBase:DoShow(ignoreAnimation: boolean): AsyncResultExtType
    local result = AsyncResultExt.new()
    local action = function (promise: AsyncResultExtType)
        if not self.created then
            self:Create()
        end

        self:OnShow()
        self.Visibility = true
        self.State = WindowEnum.WindowState.VISIBLE
        if not ignoreAnimation and self.EnterAnimation then
            self.EnterAnimation:OnStart(function ()
                self.State = WindowEnum.WindowState.ENTER_ANIMATION_BEGIN
            end):OnEnd(function ()
                self.State = WindowEnum.WindowState.ENTER_ANIMATION_END
                promise:SetResult()
            end):Play()
        else
            promise:SetResult()
        end
    end
    action(result)
    return result
end

function WindowViewBase:OnShow()
    print("parent OnShow")
end

function WindowViewBase:Hide(ignoreAnimation: boolean): TransitionType
    if not self.created then
        return
    end

    if self.dismissed then
        return
    end

    if not self.Visibility then
        return
    end

    return WindowManager.Instance:Hide(self):DisableAnimation(ignoreAnimation)
end

function WindowViewBase:DoHide(ignoreAnimation: boolean): AsyncResultExtType
    local result = AsyncResultExt.new()
    local action = function (promise: AsyncResultExtType)
        if not ignoreAnimation and self.ExitAnimation then
            self.ExitAnimation:OnStart(function ()
                self.State = WindowEnum.WindowState.EXIT_ANIMATION_BEGIN
            end):OnEnd(function ()
                self.State = WindowEnum.WindowState.EXIT_ANIMATION_END
                self.Visibility = false
                self.State = WindowEnum.WindowState.INVISIBLE
                self:OnHide()
                promise:SetResult()
            end):Play()
        else
            self.Visibility = false
            self.State = WindowEnum.WindowState.INVISIBLE
            self:OnHide()
            promise:SetResult()
        end
    end
    action(result)
    return result
end

function WindowViewBase:OnHide()
    
end

function WindowViewBase:Dismiss(ignoreAnimation: boolean): TransitionType
    if self.dismissTransition then
        return self.dismissTransition
    end

    if self.dismissed then
        return
    end

    self.dismissTransition = WindowManager.Instance:Dismiss(self):DisableAnimation(ignoreAnimation)
    return self.dismissTransition
end

function WindowViewBase:DoDismiss()
    if not self.dismissed then
        assert(self.gameObject, "")
        self.State = WindowEnum.WindowState.DISMISS_BEGIN
        self.dismissed = true
        self:OnDismiss()
        self:RaiseOnDismissed()
        CS.UnityEngine.Object.Destroy(self.gameObject.handler as CS.UnityEngine.Object)
        self.State = WindowEnum.WindowState.DISMISS_END
        self.dismissTransition = nil
    end
end

function WindowViewBase:OnDismiss()
    
end

function WindowViewBase:GetComponentWithName(component: ComponentExtType, name: string): ComponentExtType
    assert(component and name, "")
    component = component.new(self.variables:Get(name))
    table.insert(self.components, component)
    return component
end

function WindowViewBase:GetGameObjectWithName(gameObject: GameObjectExtType, name: string): GameObjectExtType
    assert(gameObject and name, "")
    gameObject = gameObject.new(self.variables:Get(name))
    table.insert(self.gameObjects, gameObject)
    return gameObject
end

function WindowViewBase:GetComponent(component: ComponentExtType): ComponentExtType
    assert(self.gameObject, "")
    return component.new(self.gameObject:GetComponent(typeof(component.CSObject)))
end

function WindowViewBase:AddComponent(component: ComponentExtType): ComponentExtType
    assert(self.gameObject, "")
    return component.new(self.gameObject:AddComponent(typeof(component.CSObject)))
end

function WindowViewBase:dtor()
    if not self.dismissed and not self.dismissTransition then
        self:Dismiss(true)
    end
    
    TableExtention.Foreach(self.components, function (_: number, v: ComponentExtType)
        v:delete()
    end)
    self.components = nil

    local layer = WindowManager.Instance:GetLayer(self.windowType)
    layer:PopWindowOder()
end

return WindowViewBase