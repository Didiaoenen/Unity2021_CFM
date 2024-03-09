local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")
local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")
local ToggleExt = require("LuaFramework.Views.Components.ToggleExt")

local BindToggle = class("BindToggle", BindComponent) as BindToggleType

function BindToggle.get:OnValueChanged(): function
    return self.onValueChanged
end

function BindToggle.set:OnValueChanged(value: function)
    self:Set({name = "onValueChanged"}, value, "OnValueChanged")
end

function BindToggle.get:SimpleCommand(): SimpleCommandType
    return self.simpleCommand
end

function BindToggle.set:SimpleCommand(value: SimpleCommandType)
    self:Set({name = "simpleCommand"}, value, "SimpleCommand")
end

function BindToggle:ctor()
    BindComponent.ctor(self)
    self.gameObject = BindGameObject.new()
    self.onValueChanged = function() end
    self.simpleCommand = SimpleCommand.new(function() end)
end

function BindToggle:BindWithName(view: ViewBaseType, name: string, modes: {number})
    self.componentExt = view:GetComponentWithName(ToggleExt as ComponentExtType, name) as ToggleExtType
    local bindingSet = view.bindingSet as BindingSetType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.OnValueChanged):To(name .. ".OnValueChanged"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.OnValueChanged):To(name .. ".SimpleCommand"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt.gameObject as LuaObject)
    builder:For(self.componentExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

function BindToggle:dtor()
end

return BindToggle