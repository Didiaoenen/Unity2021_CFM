local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local CanvasExt = require("LuaFramework.Views.Components.CanvasExt")

local BindCanvas = class("BindCanvas", BindComponent) as BindCanvasType

function BindCanvas.get:OverrideSorting(): boolean
    return self.overrideSorting
end

function BindCanvas.set:OverrideSorting(value: boolean)
    self:Set({name = "overrideSorting"}, value, "OverrideSorting")
end

function BindCanvas.get:SortingOrder(): number
    return self.sortingOrder
end

function BindCanvas.set:SortingOrder(value: number)
    self:Set({name = "sortingOrder"}, value, "SortingOrder")
end

function BindCanvas:ctor()
    BindComponent.ctor(self)
    self.overrideSorting = true
    self.sortingOrder = 0
end

function BindCanvas:BindWithName(view: ViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(CanvasExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local canvasExt = component as CanvasExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(canvasExt.OverrideSorting):To(name .. ".OverrideSorting"):SetMode(_Mode())
    
    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(canvasExt.SortingOrder):To(name .. ".SortingOrder"):SetMode(_Mode())
end

function BindCanvas:dtor()
end

return BindCanvas