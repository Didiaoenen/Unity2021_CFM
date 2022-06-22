require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local TextExt = class("TextExt", ComponentExt) as TextExtType

TextExt.CSObject = CS.UnityEngine.UI.Text as CS.UnityEngine.Component

TextExt.Text = "text"

function TextExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return TextExt