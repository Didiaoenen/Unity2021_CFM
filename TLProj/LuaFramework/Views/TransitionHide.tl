

require("OOP.Class")

local Transition = require("LuaFramework.Views.Transition")

local TransitionHide = class("TransitionHide", Transition) as TransitionHideType

function TransitionHide:ctor(manager: WindowManagerType, window: WindowBaseType, dismiss: boolean)
    Transition.ctor(self, window)
    self.dismiss = dismiss
    self.manager = manager
end

function TransitionHide:DoTransition()
    Transition.DoTransition(self)
    local current = self.window as WindowViewBaseType
    if current.Visibility then
        local hide = current:DoHide(self.AnimationDisabled)
        CoroutineExt.Yield(hide:WaitForDone())
    end

    if self.dismiss then
        current:DoDismiss()
    end

    local previous: WindowViewBaseType = nil
    if current.windowType == WindowType.NONE then
        -- nothing to do
    elseif current.windowType == WindowType.FULL then
        previous = WindowManager:PopWindowStack()
    elseif current.windowType == WindowType.POPUP or current.windowType == WindowType.DIALOG then
        previous = (self.window as WindowBaseType):PopOpenView()
    end

    if previous then
        local show = previous:DoShow(self.AnimationDisabled)
        CoroutineExt.Yield(show:WaitForDone())
    end
end

function TransitionHide:dtor()
    
end

return TransitionHide