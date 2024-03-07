local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local CanvasGroupExt = require("LuaFramework.Views.Components.CanvasGroupExt")

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

function BindCanvasGroup:BindWithName(view: ViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(CanvasGroupExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local canvasGroupExt = component as CanvasGroupExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(canvasGroupExt.Alpha):To(name .. ".Alpha"):SetMode(_Mode())
end

function BindCanvasGroup:dtor()
end

return BindCanvasGroup