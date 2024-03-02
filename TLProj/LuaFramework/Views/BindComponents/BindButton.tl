local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")

local BindButton = class("BindButton", BindComponent) as BindButtonType

function BindButton.get:OnClick(): function
    return self.onClick
end

function BindButton.set:OnClick(value: function)
    self:Set({name = "onClick"}, value, "OnClick")
end

function BindButton.get:SimpleCommand(): SimpleCommandType
    return self.simpleCommand
end

function BindButton.set:SimpleCommand(value: SimpleCommandType)
    self:Set({name = "simpleCommand"}, value, "SimpleCommand")
end

function BindButton:ctor()
    BindComponent.ctor(self)
    self.onClick = function() end
    self.simpleCommand = SimpleCommand.new(function() end)
end

function BindButton:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, modes: {number})
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local buttonExt = component as ButtonExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(buttonExt.OnClick):To(name .. ".OnClick"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(buttonExt.OnClick):To(name .. ".SimpleCommand"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(buttonExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

function BindButton:dtor()
end

return BindButton