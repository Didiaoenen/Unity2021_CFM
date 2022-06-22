require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local ButtonExt = class("ButtonExt", ComponentExt) as ButtonExtType

ButtonExt.CSObject = CS.UnityEngine.UI.Button as CS.UnityEngine.Component

ButtonExt.OnClick = "onClick"

function ButtonExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return ButtonExt