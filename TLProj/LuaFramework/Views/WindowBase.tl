require("OOP.Class")

local WindowEnum = require("LuaFramework.Views.WindowEnum")
local WindowViewBase = require("LuaFramework.Views.WindowViewBase")
local TableExtention = require("LuaFramework.Common.TableExtention")

local WindowBase = class("WindowBase", WindowViewBase) as WindowBaseType

function WindowBase.get:Current(): WindowViewBaseType
    return self.viewStack[#(self.viewStack)]
end

function WindowBase:ctor(bundle)
    WindowViewBase.ctor(self, bundle)

    self.subView = {}
    self.viewStack = {}

    self.windowType = WindowEnum.WindowType.FULL
end

function WindowBase:OnEnable()
    WindowViewBase.OnEnable(self)
end

function WindowBase:OnDisable()
    WindowViewBase.OnDisable(self)
end

function WindowBase:OnShow()
    TableExtention.Foreach(self.subView, function (_: number, v: WindowViewBaseType)
        v:DoShow()
    end)
end

function WindowBase:OnHide()
    TableExtention.Foreach(self.subView, function (_: number, v: WindowViewBaseType)
        v:DoHide()
    end)
    TableExtention.Foreach(self.viewStack, function (_: number, v: WindowViewBaseType)
        v:DoDismiss()
    end)
end

function WindowBase:AddSubView(view: WindowViewBaseType)
    table.insert(self.subView, view)
end

function WindowBase:PushViewStack(view: WindowViewBaseType): WindowViewBaseType
    local preCurrent = self.Current
    if self.Current and view.WindowType == self.Current.WindowType then
        if view.Mutex then
            preCurrent = self:PopViewStack()
        end
    end
    table.insert(self.viewStack, view)
    return preCurrent
end

function WindowBase:PopViewStack(): WindowViewBaseType
    assert(#(self.viewStack) > 0)
    table.remove(self.viewStack, #(self.viewStack))
    return self.Current
end

function WindowBase:dtor()
    TableExtention.Foreach(self.subView, function (_: number, v: WindowViewBaseType)
        v:delete()
    end)
    self.subView = nil
end

return WindowBase