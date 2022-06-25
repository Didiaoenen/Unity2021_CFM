require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local GraphicRaycasterExt = class("GraphicRaycasterExt", ComponentExt) as GraphicRaycasterExtType

GraphicRaycasterExt.CSObject = CS.UnityEngine.UI.GraphicRaycaster as CS.UnityEngine.Component

function GraphicRaycasterExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

return GraphicRaycasterExt