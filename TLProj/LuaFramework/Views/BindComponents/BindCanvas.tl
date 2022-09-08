local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")

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

function BindCanvas:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local canvasExt = component as CanvasExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(canvasExt.OverrideSorting):To(name .. ".OverrideSorting"):SetMode(mode)
    
    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(canvasExt.SortingOrder):To(name .. ".SortingOrder"):SetMode(mode)
end

return BindCanvas