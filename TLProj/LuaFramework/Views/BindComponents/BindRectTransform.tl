local BindTransform = require("LuaFramework.Views.BindComponents.BindTransform")
local RectTransformExt = require("LuaFramework.Views.Components.RectTransformExt")

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

function BindRectTransform:BindWithName(view: WindowViewBaseType, name: string, modes: {number})
    BindTransform.BindWithName(self as BindTransformType, view, name, modes)
    
    local component = view:GetComponentWithName(RectTransformExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local rectTransformExt = component as RectTransformExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end
    
    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(rectTransformExt.OffsetMax):To(name .. ".OffsetMax"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(rectTransformExt.OffsetMin):To(name .. ".OffsetMin"):SetMode(_Mode())
end

function BindRectTransform:dtor()
end

return BindRectTransform