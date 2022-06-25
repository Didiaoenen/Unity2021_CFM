require("OOP.Class")

local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")

local ObservableObject = require("LuaFramework.Observables.ObservableObject")

local BindText = class("BindText", ObservableObject) as BindTextType

function BindText.get:Text(): string
    return self.text
end

function BindText.set:Text(value: string)
    self:Set({name = "text"}, value, "Text")
end

function BindText.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindText:ctor()
    ObservableObject.ctor(self)
    self.text = ""
    self.gameObject = BindGameObject.new()
end

return BindText