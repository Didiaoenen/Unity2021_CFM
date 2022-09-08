local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")

local BindInputField = class("BindInputField", BindComponent) as BindInputFieldType

function BindInputField.get:Text(): string
    return self.text
end

function BindInputField.set:Text(value: string)
    self:Set({name = "text"}, value, "Text")
end

function BindInputField:ctor()
    BindComponent.ctor(self)
    self.text = ""
end

function BindInputField:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local inputFieldExt = component as InputFieldExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(inputFieldExt.Text, inputFieldExt.OnValueChanged):To(name .. ".Text"):SetMode(mode)

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(inputFieldExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(mode)
end

return BindInputField