

require("OOP.Class")

local WindowViewBase = require("LuaFramework.Views.WindowViewBase")

local WindowBase = class("WindowBase", WindowViewBase) as WindowBaseType

function WindowBase.get:Current(): WindowViewBaseType
    return self.viewStack[#(self.viewStack)]
end

function WindowBase:ctor(bundle)
    WindowViewBase.ctor(self, bundle)

    self.subView = {}
    self.viewStack = {}

    self.windowType = WindowType.FULL
end

function WindowBase:OnEnable()
    WindowViewBase.OnEnable(self)
end

function WindowBase:OnDisable()
    WindowViewBase.OnDisable(self)
end

function WindowBase:OnShow()
    TableExtention.Foreach(self.subView, function (i: number, v: WindowBaseType)
        v:OnShow()
    end)
end

function WindowBase:OnHide()
    TableExtention.Foreach(self.subView, function (i: number, v: WindowBaseType)
        v:OnHide()
    end)
end

function WindowBase:CreateSubView(name: string, bundle: any): WindowViewBaseType
    assert(name, "")
    return WindowManager:Open(name, bundle)
end

function WindowBase:AddSubView(view: WindowViewBaseType)
    table.insert(self.subView, view)
end

function WindowBase:PushOpenView(view: WindowViewBaseType): WindowViewBaseType
    local preCurrent = self.Current
    if self.Current and view.WindowType == self.Current.WindowType then
        if view.Mutex then
            preCurrent = self:PopOpenView()
        end
    end
    table.insert(self.viewStack, view)
    return preCurrent
end

function WindowBase:PopOpenView(): WindowViewBaseType
    assert(#(self.viewStack) > 0)
    table.remove(self.viewStack, #(self.viewStack))
    return self.Current
end

function WindowBase:dtor()
    TableExtention.Foreach(self.subView, function (i: number, v: WindowViewBaseType)
        v:delete()
    end)
    self.subView = nil
end

return WindowBase