

require("OOP.Class")

local CoroutineExt: CoroutineExtType
local WindowManager: WindowManagerType

local Transition = require("LuaFramework.Views.Transition")

local TransitionShow = class("TransitionShow", Transition) as WindowManager.TransitionShowType

function TransitionShow:ctor(manager: WindowManager, window: WindowBaseType)
    Transition.ctor(self, window)
    self.manager = manager
end

function TransitionShow:Overlay(previous: WindowViewBaseType): number
    if previous.WindowType == WindowType.FULL then
        return ActionType.Hide
    end

    if previous.WindowType == WindowType.POPUP then
        return ActionType.Dismiss
    end

    return ActionType.None
end

function TransitionShow:DoTransition()
    Transition.DoTransition(self)
    local current = self.window

    local previous: WindowViewBaseType = nil
    if current.windowType == WindowType.NONE then
        (WindowManager.Current as WindowBaseType):AddSubView(current)
    elseif current.windowType == WindowType.FULL then
        previous = WindowManager:PushWindowStack(current) as WindowViewBaseType
    elseif current.windowType == WindowType.POPUP or self.window.windowType == WindowType.DIALOG then
        previous = (WindowManager.Current as WindowBaseType):PushViewStack(current) as WindowViewBaseType
    end

    if previous then
        local policy = self.OverlayPolicy
        if not policy then
            policy = self.Overlay
        end

        local actionType = policy(self, previous, current)
        if actionType == ActionType.Hide then
            local hide = previous:DoHide(self.AnimationDisabled)
            CoroutineExt.Yield(hide:WaitForDone())
        elseif actionType == ActionType.Dismiss then
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

return TransitionShow