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

function BindInputField:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, modes: {number})
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local inputFieldExt = component as InputFieldExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(inputFieldExt.Text, inputFieldExt.OnValueChanged):To(name .. ".Text"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(inputFieldExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

return BindInputField