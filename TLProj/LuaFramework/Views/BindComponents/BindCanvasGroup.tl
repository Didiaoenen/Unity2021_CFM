require("OOP.Class")

local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")

local BindCanvasGroup = class("BindCanvasGroup", BindComponent) as BindCanvasGroupType

function BindCanvasGroup.get:Alpha(): number
    return self.alpha
end

function BindCanvasGroup.set:Alpha(value: number)
    self:Set({name = "alpha"}, value, "Alpha")
end

function BindCanvasGroup:ctor()
    BindComponent.ctor(self)
    self.alpha = 1
end

function BindCanvasGroup:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local canvasGroupExt = component as CanvasGroupExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(canvasGroupExt.Alpha):To(name .. ".Alpha"):SetMode(mode)
end

return BindCanvasGroup