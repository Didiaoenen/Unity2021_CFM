

require("OOP.Class")

local TableExtention: TableExtentionType

local Layer = require("Luaframework.Views.Layer")
local TransitionShow = require("Luaframework.Views.TransitionShow")
local TransitionHide = require("Luaframework.Views.TransitionHide")
local TransitionExecutor = require("Luaframework.Views.TransitionExecutor")

local WindowManager = class("WindowManager") as WindowManagerType

local layers = {
	{WindowType = WindowType.NONE, OrderInLayer = 0, OrderStep = 10},
	{WindowType = WindowType.FULL, OrderInLayer = 1000, OrderStep = 10},
	{WindowType = WindowType.POPUP, OrderInLayer = 2000, OrderStep = 10},
	{WindowType = WindowType.DIALOG, OrderInLayer = 3000, OrderStep = 10},
	{WindowType = WindowType.PROGRESS, OrderInLayer = 4000, OrderStep = 10},
	{WindowType = WindowType.QUEUED_POPUP, OrderInLayer = 5000, OrderStep = 10},
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
    return (transition as TransitionType):OnStateChanged(function (w: WindowViewBaseType, state: number)
        if state == WindowState.VISIBLE then
        end
    end)
end

function WindowManager:Hide(window: WindowViewBaseType): TransitionType
    local transition = TransitionHide.new(self, window, false)
    GetTransitionExecutor():Execute(transition as TransitionType)
    return (transition as TransitionType):OnStateChanged(function (w: WindowViewBaseType, state: number)
        if state == WindowState.INVISIBLE then
        end
    end)
end

function WindowManager:Dismiss(window: WindowViewBaseType): TransitionType
    local transition = TransitionHide.new(self, window, true)
    GetTransitionExecutor():Execute(transition as TransitionType)
    return (transition as TransitionType):OnStateChanged(function (w: WindowViewBaseType, state: number)
        if state == WindowState.INVISIBLE then
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
    return TableExtention.Find(self.windowViews, function (i: number, v: WindowViewBaseType): boolean
        return v.Name == name
    end) as WindowViewBaseType
end

function WindowManager:CreateLayers()
    TableExtention.Foreach(layers, function (i: number, v: LayerType)
        local layer = Layer.new(v)
        table.insert(self.layers, layer)
    end)
end

function WindowManager:GetLayer(windowType: number): LayerType
    return TableExtention.Find(self.layers, function (i: number, v: LayerType): boolean
        return v.WindowType == windowType
    end) as LayerType
end

function WindowManager:dtor()
end

return WindowManager