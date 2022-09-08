local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local ButtonExt = class("ButtonExt", ComponentExt) as ButtonExtType

ButtonExt.CSObject = CS.UnityEngine.UI.Button as CS.UnityEngine.Component

ButtonExt.OnClick = "onClick"

function ButtonExt.get:onClick(): function
    return (self.handler as CS.UnityEngine.UI.Button).onClick
end

function ButtonExt.set:onClick(value: function)
    (self.handler as CS.UnityEngine.UI.Button).onClick = value
end

function ButtonExt.get:GameObject(): GameObjectExtType
    return self.gameObject
end

function ButtonExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

return ButtonExt