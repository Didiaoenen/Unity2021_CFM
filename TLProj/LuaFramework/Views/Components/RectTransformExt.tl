local TransformExt = require("LuaFramework.Views.Components.TransformExt")

local RectTransformExt = class("RectTransformExt", TransformExt) as RectTransformExtType

RectTransformExt.CSObject = CS.UnityEngine.RectTransform as CS.UnityEngine.Component

RectTransformExt.OffsetMax = "offsetMax"

RectTransformExt.OffsetMin = "offsetMin"

function RectTransformExt.get:offsetMax(): CS.UnityEngine.Vector2
    return (self.handler as CS.UnityEngine.RectTransform).offsetMax
end

function RectTransformExt.set:offsetMax(value: CS.UnityEngine.Vector2)
    (self.handler as CS.UnityEngine.RectTransform).offsetMax = value
end

function RectTransformExt.get:offsetMin(): CS.UnityEngine.Vector2
    return (self.handler as CS.UnityEngine.RectTransform).offsetMin
end

function RectTransformExt.set:offsetMin(value: CS.UnityEngine.Vector2)
    (self.handler as CS.UnityEngine.RectTransform).offsetMin = value
end

function RectTransformExt:ctor(handler: CS.UnityEngine.Component)
    TransformExt.ctor(self, handler)
end

return RectTransformExt