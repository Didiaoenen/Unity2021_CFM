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

function BindRectTransform:BindWithName(view: ViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(RectTransformExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end
    
    self:BindToTarget(bindingSet, name, component, _Mode())
end

function BindRectTransform:BindToTarget(bindingSet: BindingSetType, name: string, target: ComponentExtType, mode: number)
    BindTransform.BindToTarget(self as BindTransformType, bindingSet, name, target, mode)

    local rectTransform = target as RectTransformExtType
    local builder = bindingSet:BindByTarget(target as LuaObject)
    builder:For(rectTransform.OffsetMax):To(name .. ".OffsetMax"):SetMode(mode)

    builder = bindingSet:BindByTarget(target as LuaObject)
    builder:For(rectTransform.OffsetMin):To(name .. ".OffsetMin"):SetMode(mode)
end

function BindRectTransform:dtor()
end

return BindRectTransform