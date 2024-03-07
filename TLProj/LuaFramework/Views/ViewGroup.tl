local ListExtention = require("LuaFramework.Common.ListExtention")

local ViewGroup = class("ViewGroup") as ViewGroupType

function ViewGroup:ctor()
    self.views = {}
end

function ViewGroup:OnEnable()
end

function ViewGroup:OnDisable()
end

function ViewGroup:OnShow()
    ListExtention.Foreach(self.views, function (_: number, v: ViewBaseType)
        v:DoShow()
    end)
end

function ViewGroup:OnHide()
    ListExtention.Foreach(self.views, function (_: number, v: ViewBaseType)
        v:DoHide()
    end)
    ListExtention.Foreach(self.views, function (_: number, v: ViewBaseType)
        v:DoDismiss()
    end)
end

function ViewGroup:GetView(name: string): ViewBaseType
    assert({name}, "")
end

function ViewGroup:AddView(name: string, parent: RectTransformExtType)
    assert({name, parent}, "")
end

function ViewGroup:RemoveView(name: string)
    assert({name}, "")
end

function ViewGroup:ctor()
end

return ViewGroup