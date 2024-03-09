local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")
local InputFieldExt = require("LuaFramework.Views.Components.InputFieldExt")
local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")

local BindInputField = class("BindInputField", BindComponent) as BindInputFieldType

function BindInputField.get:Text(): string
    return self.text
end

function BindInputField.set:Text(value: string)
    self:Set({name = "text"}, value, "Text")
end

function BindInputField.get:OnValueChanged(): function
    return self.onValueChanged
end

function BindInputField.set:OnValueChanged(value: function)
    self:Set({name = "onValueChanged"}, value, "OnValueChanged")
end

function BindInputField.get:SimpleCommand(): SimpleCommandType
    return self.simpleCommand
end

function BindInputField.set:SimpleCommand(value: SimpleCommandType)
    self:Set({name = "simpleCommand"}, value, "SimpleCommand")
end

function BindInputField:ctor()
    BindComponent.ctor(self)
    self.gameObject = BindGameObject.new()
    self.text = ""
    self.onValueChanged = function() end
    self.simpleCommand = SimpleCommand.new(function() end)
end

function BindInputField:BindWithName(view: ViewBaseType, name: string, modes: {number})
    self.componentExt = view:GetComponentWithName(InputFieldExt as ComponentExtType, name) as InputFieldExtType
    local bindingSet = view.bindingSet as BindingSetType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.Text, self.componentExt.OnEndEdit):To(name .. ".Text"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.OnValueChanged):To(name .. ".OnValueChanged"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.OnValueChanged):To(name .. ".SimpleCommand"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt.gameObject as LuaObject)
    builder:For(self.componentExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

function BindInputField:dtor()
end

return BindInputField