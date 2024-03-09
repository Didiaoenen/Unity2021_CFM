local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local ButtonExt = class("ButtonExt", ComponentExt) as ButtonExtType

ButtonExt.CSObject = CS.UnityEngine.UI.Button as CS.UnityEngine.Component

ButtonExt.OnClick = "onClick"

function ButtonExt.get:onClick(): function
    local handler = self.handler as CS.UnityEngine.UI.Button
    return handler.onClick
end

function ButtonExt.set:onClick(value: function)
    local handler = self.handler as CS.UnityEngine.UI.Button
    handler.onClick = value
end

function ButtonExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

function ButtonExt:dtor()
end

return ButtonExt