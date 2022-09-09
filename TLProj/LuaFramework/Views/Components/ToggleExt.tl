local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local ToggleExt = class("ToggleExt", ComponentExt) as ToggleExtType

ToggleExt.CSObject = CS.UnityEngine.UI.Toggle as CS.UnityEngine.Component

ToggleExt.OnValueChanged = "onValueChanged"

function ToggleExt.get:onValueChanged(): CS.UnityEngine.UI.Toggle.ToggleEvent
    return (self.handler as CS.UnityEngine.UI.Toggle).onValueChanged
end

function ToggleExt.set:onValueChanged(value: CS.UnityEngine.UI.Toggle.ToggleEvent)
    (self.handler as CS.UnityEngine.UI.Toggle).onValueChanged = value
end

function ToggleExt.get:GameObject(): GameObjectExtType
    return self.gameObject
end

function ToggleExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

return ToggleExt