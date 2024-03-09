local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
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
    self.onValueChanged = function() end
    self.simpleCommand = SimpleCommand.new(function() end)
end

function BindToggle:BindWithName(view: ViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(ToggleExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local toggleExt = component as ToggleExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(toggleExt.OnValueChanged):To(name .. ".OnValueChanged"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(toggleExt.OnValueChanged):To(name .. ".SimpleCommand"):SetMode(_Mode())

    self.gameObject:BindToTarget(bindingSet, name .. ".GameObject.ActiveSelf", toggleExt.gameObject as ComponentExtType, _Mode())
end

function BindToggle:dtor()
end

return BindToggle