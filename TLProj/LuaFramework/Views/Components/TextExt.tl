require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")
local GameObjectExt = require("LuaFramework.Views.Components.GameObjectExt")

local TextExt = class("TextExt", ComponentExt) as TextExtType

TextExt.CSObject = CS.UnityEngine.UI.Text as CS.UnityEngine.Component

TextExt.Text = "text"

function TextExt.get:text(): string
    return (self.handler as CS.UnityEngine.UI.Text).text
end

function TextExt.set:text(value: string)
    (self.handler as CS.UnityEngine.UI.Text).text = value
end

function TextExt.get:GameObject(): GameObjectExtType
    return self.gameObject
end

function TextExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)

    self.gameObject = GameObjectExt.new(handler.gameObject)
end

return TextExt