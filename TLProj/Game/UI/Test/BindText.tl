require("OOP.Class")

local BindGameObject = require("Game.UI.Test.BindGameObject")

local BindText = class("BindText") as BindTextType

function BindText.get:Text(): string
    return self.text
end

function BindText.set:Text(value: string)
    self.text = value
end

function BindText.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindText.set:GameObject(value: BindGameObjectType)
    self.gameObject = value
end

function BindText:ctor()
    self.text = ""
    self.gameObject = BindGameObject.new()
end

return BindText