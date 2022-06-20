

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local TransformExt = class("TransformExt", ComponentExt) as TransformExtType

TransformExt.CSObject = CS.CSObject.Transform as Component

TransformExt.LocalRotation = "localRotation"

TransformExt.LocalPosition = "localPosition"

TransformExt.LocalScale = "localScale"

TransformExt.Parent = "parent"

function TransformExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

return TransformExt