

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local CanvasExt = class("CanvasExt", Component) as CanvasExtType

CanvasExt.CSharpType = CSharpType.Canvas as Component

CanvasExt.OverrideSorting = "overrideSorting"

CanvasExt.SortingOrder = "sortingOrder"

function CanvasExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return CanvasExt