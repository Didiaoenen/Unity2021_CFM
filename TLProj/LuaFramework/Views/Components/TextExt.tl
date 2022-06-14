

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local TextExt = class("TextExt", ComponentExt) as TextExtType

TextExt.CSharpType = CSharpType.Text as Component

TextExt.Text = "text"

function TextExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return TextExt