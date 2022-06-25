require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local CanvasExt = class("CanvasExt", ComponentExt) as CanvasExtType

CanvasExt.CSObject = CS.UnityEngine.Canvas as CS.UnityEngine.Component

CanvasExt.OverrideSorting = "overrideSorting"

CanvasExt.SortingOrder = "sortingOrder"

function CanvasExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

return CanvasExt