require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local TransformExt = class("TransformExt", ComponentExt) as TransformExtType

TransformExt.CSObject = CS.UnityEngine.Transform as CS.UnityEngine.Component

TransformExt.LocalRotation = "localRotation"

TransformExt.LocalPosition = "localPosition"

TransformExt.LocalScale = "localScale"

TransformExt.Parent = "parent"

function TransformExt.get:localRotation(): CS.UnityEngine.Quaternion
    return (self.handler as CS.UnityEngine.Transform).localRotation
end

function TransformExt.set:localRotation(value: CS.UnityEngine.Quaternion)
    (self.handler as CS.UnityEngine.Transform).localRotation = value
end

function TransformExt.get:localPosition(): CS.UnityEngine.Vector3
    return (self.handler as CS.UnityEngine.Transform).localPosition
end

function TransformExt.set:localPosition(value: CS.UnityEngine.Vector3)
    (self.handler as CS.UnityEngine.Transform).localPosition = value
end

function TransformExt.get:localScale(): CS.UnityEngine.Vector3
    return (self.handler as CS.UnityEngine.Transform).localScale
end

function TransformExt.set:localScale(value: CS.UnityEngine.Vector3)
    (self.handler as CS.UnityEngine.Transform).localScale = value
end

function TransformExt.set:parent(value: CS.UnityEngine.Transform)
    (self.handler as CS.UnityEngine.Transform).parent = value
end

function TransformExt.get:parent(): CS.UnityEngine.Transform
    return (self.handler as CS.UnityEngine.Transform).parent
end

function TransformExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

function TransformExt:SetParent(parent: TransformExtType)
    (self.handler as CS.UnityEngine.Transform):SetParent(parent.handler as CS.UnityEngine.Transform)
end

return TransformExt