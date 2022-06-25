require("OOP.Class")

local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")

local BindButton = class("BindButton") as BindButtonType

function BindButton.get:OnClick(): function
    return self.onClick
end

function BindButton.set:OnClick(value: function)
    self.onClick = value
end

function BindButton.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindButton.set:GameObject(value: BindGameObjectType)
    self.gameObject = value
end

function BindButton:ctor()
    self.onClick = self.OnClick
    self.gameObject = BindGameObject.new()
    self.gameObject.ActiveSelf = false
end

function BindButton:OnClick()
end

return BindButton