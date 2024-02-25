local Layer = require("Luaframework.Views.Layer")
local WindowEnum = require("LuaFramework.Views.WindowEnum")
local ListExtention = require("LuaFramework.Common.ListExtention")
local TransitionExecutor = require("Luaframework.Views.TransitionExecutor")

local Transition = require("LuaFramework.Views.Transition")
local CoroutineExt = require("LuaFramework.Common.CoroutineExt")

local WindowManager = class("WindowManager") as WindowManagerType

local TransitionShow = class("TransitionShow", Transition) as WindowManager.TransitionShowType

local TransitionHide = class("TransitionHide", Transition) as WindowManagerType.TransitionHideType

local layers = {
	{WindowType = WindowEnum.WindowType.NONE, OrderInLayer = 0, OrderStep = 10},
	{WindowType = WindowEnum.WindowType.FULL, OrderInLayer = 1000, OrderStep = 10},
	{WindowType = WindowEnum.WindowType.POPUP, OrderInLayer = 2000, OrderStep = 10},
	{WindowType = WindowEnum.WindowType.DIALOG, OrderInLayer = 3000, OrderStep = 10},
	{WindowType = WindowEnum.WindowType.PROGRESS, OrderInLayer = 4000, OrderStep = 10},
	{WindowType = WindowEnum.WindowType.QUEUED_POPUP, OrderInLayer = 5000, OrderStep = 10},
}

local blockingExecutor: TransitionExecutorType = nil

local GetTransitionExecutor = function (): TransitionExecutorType
    if not blockingExecutor then
        blockingExecutor = TransitionExecutor.new()
    end
    return blockingExecutor
end

function WindowManager.get:Current(): WindowViewBaseType
    return self.windowStack[#(self.windowStack)]
end

function WindowManager:ctor()
    self.current = nil

    self.layers = {}
    self.windowViews = {}
    self.windowStack = {}
    self.hideWindows = {}

    self:CreateLayers()
end

function WindowManager:__singleton(): WindowManager
    return WindowManager.new()
end

function WindowManager:OnEnable()

end

function WindowManager:OnDisable()

end

function WindowManager:Show(window: WindowViewBaseType): TransitionType
    local transition = TransitionShow.new(self, window)
    GetTransitionExecutor():Execute(transition as TransitionType)
    return (transition as TransitionType):OnStateChanged(function (_: WindowViewBaseType, state: number)
        if state == WindowEnum.WindowState.VISIBLE then
        end
    end)
end

function WindowManager:Hide(window: WindowViewBaseType): TransitionType
    local transition = TransitionHide.new(self, window, false)
    GetTransitionExecutor():Execute(transition as TransitionType)
    return (transition as TransitionType):OnStateChanged(function (_: WindowViewBaseType, state: number)
        if state == WindowEnum.WindowState.INVISIBLE then
        end
    end)
end

function WindowManager:Dismiss(window: WindowViewBaseType): TransitionType
    local transition = TransitionHide.new(self, window, true)
    GetTransitionExecutor():Execute(transition as TransitionType)
    return (transition as TransitionType):OnStateChanged(function (_: WindowViewBaseType, state: number)
        if state == WindowEnum.WindowState.INVISIBLE then
        end
    end)
end

function WindowManager:PushWindowStack(window: WindowViewBaseType): WindowViewBaseType
    local preCurrent = self.Current
    if #(self.windowStack) > 0 then
        if preCurrent ~= self.windowStack[1] then
            preCurrent = self:PopWindowStack()
        end
    end
    table.insert(self.windowStack, window)
    return preCurrent
end

function WindowManager:PopWindowStack(): WindowViewBaseType
    assert(#(self.windowStack) > 0, "")
    table.remove(self.windowStack, #(self.windowStack))
    return self.Current
end

function WindowManager:ClearWindowStack()
    for i = #(self.windowStack), 1, -1 do
        self.windowStack[i]:OnDismiss()
    end
end

function WindowManager:GetWindow(name: string): WindowViewBaseType
    return ListExtention.Find(self.windowViews, function (_: number, v: WindowViewBaseType): boolean
        return v.Name == name
    end) as WindowViewBaseType
end

function WindowManager:CreateLayers()
    ListExtention.Foreach(layers, function (_: number, v: LayerType)
        local layer = Layer.new(v)
        table.insert(self.layers, layer)
    end)
end

function WindowManager:GetLayer(windowType: number): LayerType
    return ListExtention.Find(self.layers, function (_: number, v: LayerType): boolean
        return v.WindowType == windowType
    end) as LayerType
end

function WindowManager:dtor()
end

----------------------------------------------
function TransitionShow:ctor(manager: WindowManager, window: WindowBaseType)
    Transition.ctor(self, window)
    self.manager = manager
end

function TransitionShow:Overlay(previous: WindowViewBaseType): number
    if previous.WindowType == WindowEnum.WindowType.FULL then
        return WindowEnum.ActionType.Hide
    end

    if previous.WindowType == WindowEnum.WindowType.POPUP then
        return WindowEnum.ActionType.Dismiss
    end

    return WindowEnum.ActionType.None
end

function TransitionShow:DoTransition()
    Transition.DoTransition(self as TransitionType)
    local current = self.window

    local previous: WindowViewBaseType = nil
    if current.windowType == WindowEnum.WindowType.NONE then
        (WindowManager.Instance.Current as WindowBaseType):AddSubView(current)
    elseif current.windowType == WindowEnum.WindowType.FULL then
        previous = WindowManager.Instance:PushWindowStack(current) as WindowViewBaseType
    elseif current.windowType == WindowEnum.WindowType.POPUP or self.window.windowType == WindowEnum.WindowType.DIALOG then
        previous = (WindowManager.Instance.Current as WindowBaseType):PushViewStack(current) as WindowViewBaseType
    end

    if previous then
        local policy = self.OverlayPolicy
        if not policy then
            policy = self.Overlay
        end

        local actionType = policy(self, previous, current)
        if actionType == WindowEnum.ActionType.Hide then
            local hide = previous:DoHide(self.AnimationDisabled)
            CoroutineExt.Yield(hide:WaitForDone())
        elseif actionType == WindowEnum.ActionType.Dismiss then
            local hide = previous:DoHide(self.AnimationDisabled)
            CoroutineExt.Yield(hide:WaitForDone())
            previous:DoDismiss()
        end
    end

    if not current.Visibility then
        local show = current:DoShow(self.AnimationDisabled)
        CoroutineExt.Yield(show:WaitForDone())
    end
end

function TransitionShow:dtor()
end
----------------------------------------------

----------------------------------------------
function TransitionHide:ctor(manager: WindowManager, window: WindowBaseType, dismiss: boolean)
    Transition.ctor(self, window)
    self.dismiss = dismiss
    self.manager = manager
end

function TransitionHide:DoTransition()
    Transition.DoTransition(self as TransitionType)
    local current = self.window as WindowViewBaseType
    if current.Visibility then
        local hide = current:DoHide(self.AnimationDisabled)
        CoroutineExt.Yield(hide:WaitForDone())
    end

    if self.dismiss then
        current:DoDismiss()
    end

    local previous: WindowViewBaseType = nil
    if current.windowType == WindowEnum.WindowType.NONE then
        -- nothing to do
    elseif current.windowType == WindowEnum.WindowType.FULL then
        previous = WindowManager.Instance:PopWindowStack()
    elseif current.windowType == WindowEnum.WindowType.POPUP or current.windowType == WindowEnum.WindowType.DIALOG then
        previous = (self.window as WindowBaseType):PopViewStack()
    end

    if previous then
        local show = previous:DoShow(self.AnimationDisabled)
        CoroutineExt.Yield(show:WaitForDone())
    end
end

function TransitionHide:dtor()
end
----------------------------------------------

return WindowManager