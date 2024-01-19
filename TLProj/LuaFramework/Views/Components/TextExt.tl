local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local TextExt = class("TextExt", ComponentExt) as TextExtType

TextExt.CSObject = CS.UnityEngine.UI.Text as CS.UnityEngine.Component

TextExt.Text = "text"

TextExt.Color = "color"

function TextExt.get:text(): string
    return (self.handler as CS.UnityEngine.UI.Text).text
end

function TextExt.set:text(value: string)
    (self.handler as CS.UnityEngine.UI.Text).text = value
end

function TextExt.get:color(): CS.UnityEngine.Color
    return (self.handler as CS.UnityEngine.UI.Text).color
end

function TextExt.set:color(value: CS.UnityEngine.Color)
    (self.handler as CS.UnityEngine.UI.Text).color = value
end

function TextExt.get:GameObject(): GameObjectExtType
    return self.gameObject
end

function TextExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

function TextExt:dtor()
end

return TextExt