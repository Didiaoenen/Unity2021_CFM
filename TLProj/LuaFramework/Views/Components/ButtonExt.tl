

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local ButtonExt = class("ButtonExt", ComponentExt) as ButtonExtType

ButtonExt.CSObject = CS.CSObject.Button as Component

ButtonExt.OnClick = "onClick"

function ButtonExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return ButtonExt