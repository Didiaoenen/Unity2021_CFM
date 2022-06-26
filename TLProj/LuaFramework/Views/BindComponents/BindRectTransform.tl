require("OOP.Class")

local BindTransform = require("LuaFramework.Views.BindComponents.BindTransform")

local BindRectTransform = class("BindRectTransform", BindTransform) as BindRectTransformType

function BindRectTransform.get:OffsetMax(): CS.UnityEngine.Vector2
    return self.offsetMax
end

function BindRectTransform.set:OffsetMax(value: CS.UnityEngine.Vector2)
    self:Set({name = "offsetMax"}, value, "OffsetMax")
end

function BindRectTransform.get:OffsetMin(): CS.UnityEngine.Vector2
    return self.offsetMin
end

function BindRectTransform.set:OffsetMin(value: CS.UnityEngine.Vector2)
    self:Set({name = "offsetMin"}, value, "OffsetMin")
end

function BindRectTransform:ctor()
    BindTransform.ctor(self)
    self.offsetMax = CS.UnityEngine.Vector2.zero
    self.offsetMin = CS.UnityEngine.Vector2.zero
end

function BindRectTransform:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local rectTransformExt = component as RectTransformExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(rectTransformExt.OffsetMax):To(name .. ".OffsetMax"):SetMode(mode)

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(rectTransformExt.OffsetMin):To(name .. ".OffsetMin"):SetMode(mode)
end

return BindRectTransform