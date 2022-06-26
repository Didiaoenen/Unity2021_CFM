require("OOP.Class")

local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")

local BindButton = class("BindButton", BindComponent) as BindButtonType

function BindButton.get:OnClick(): function
    return self.onClick
end

function BindButton.set:OnClick(value: function)
    self.onClick = value
end

function BindButton.get:SimpleCommand(): SimpleCommandType
    return self.simpleCommand
end

function BindButton.set:SimpleCommand(value: SimpleCommandType)
    self.simpleCommand = value
end

function BindButton:ctor()
    BindComponent.ctor(self)
end

function BindButton:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local buttonExt = component as ButtonExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(buttonExt.OnClick):To(name .. ".OnClick"):SetMode(mode)

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(buttonExt.OnClick):To(name .. ".SimpleCommand"):SetMode(mode)

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(buttonExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(mode)
end

return BindButton