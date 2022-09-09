local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")

local BindToggle = class("BindToggle", BindComponent) as BindToggleType

function BindToggle.get:OnValueChanged(): function
    return self.onValueChanged
end

function BindToggle.set:OnValueChanged(value: function)
    self:Set({name = "onValueChanged"}, value, "OnValueChanged")
end

function BindToggle:ctor()
    BindComponent.ctor(self)
end

function BindToggle:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, modes: {number})
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local toggleExt = component as ToggleExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(toggleExt.OnValueChanged):To(name .. ".OnValueChanged"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(toggleExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

function BindToggle:dtor()
end

return BindToggle