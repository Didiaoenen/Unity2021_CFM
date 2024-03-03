local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local TransformExt = require("LuaFramework.Views.Components.TransformExt")

local BindTransform = class("BindTransform", BindComponent) as BindTransformType

function BindTransform.get:Parent(): TransformExtType
    return self.parent
end

function BindTransform.set:Parent(value: TransformExtType)
    self:Set({name = "parent"}, value, "Parent")
end

function BindTransform.get:LocalRotation(): CS.UnityEngine.Quaternion
    return self.localRotation
end

function BindTransform.set:LocalRotation(value: CS.UnityEngine.Quaternion)
    self:Set({name = "localRotation"}, value, "LocalRotation")
end

function BindTransform.get:LocalPosition(): CS.UnityEngine.Vector3
    return self.localPosition
end

function BindTransform.set:LocalPosition(value: CS.UnityEngine.Vector3)
    self:Set({name = "localPosition"}, value, "LocalPosition")
end

function BindTransform.get:LocalScale(): CS.UnityEngine.Vector3
    return self.localScale
end

function BindTransform.set:LocalScale(value: CS.UnityEngine.Vector3)
    self:Set({name = "localScale"}, value, "LocalScale")
end

function BindTransform:ctor()
    BindComponent.ctor(self)
    self.parent = nil
    self.localRotation = CS.UnityEngine.Quaternion.identity
    self.localPosition = CS.UnityEngine.Vector3.zero
    self.localScale = CS.UnityEngine.Vector3.one
end

function BindTransform:BindWithName(view: WindowViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(TransformExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local transformExt = component as TransformExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(transformExt.Parent):To(name .. ".Parent"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(transformExt.LocalRotation):To(name .. ".LocalRotation"):SetMode(_Mode())
   
    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(transformExt.LocalPosition):To(name .. ".LocalPosition"):SetMode(_Mode())
   
    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(transformExt.LocalScale):To(name .. ".LocalScale"):SetMode(_Mode())
end

function BindTransform:dtor()
end

return BindTransform