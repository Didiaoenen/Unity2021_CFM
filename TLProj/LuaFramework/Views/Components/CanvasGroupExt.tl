

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local CanvasGroupExt = class("CanvasGroupExt", Component) as CanvasGroupExtType

CanvasGroupExt.CSharpType = CSharpType.CanvasGroup as Component

CanvasGroupExt.Alpha = "alpha"

function CanvasGroupExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return CanvasGroupExt