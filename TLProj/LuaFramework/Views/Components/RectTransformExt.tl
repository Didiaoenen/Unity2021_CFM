

require("OOP.Class")

local TransformExt = require("LuaFramework.Views.Components.TransformExt")

local RectTransformExt = class("RectTransformExt", TransformExt) as RectTransformExtType

RectTransformExt.CSObject = CS.CSObject.RectTransform as Component

RectTransformExt.OffsetMax = "offsetMax"

RectTransformExt.OffsetMin = "offsetMin"

function RectTransformExt:ctor(handler)
    TransformExt.ctor(self, handler)
end

return RectTransformExt