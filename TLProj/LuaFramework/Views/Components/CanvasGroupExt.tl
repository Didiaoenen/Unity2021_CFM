

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local CanvasGroupExt = class("CanvasGroupExt", ComponentExt) as CanvasGroupExtType

CanvasGroupExt.CSObject = CS.CSObject.CanvasGroup as Component

CanvasGroupExt.Alpha = "alpha"

function CanvasGroupExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return CanvasGroupExt