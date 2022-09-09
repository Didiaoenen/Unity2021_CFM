local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local ImageExt = class("ImageExt", ComponentExt) as ImageExtType

ImageExt.CSObject = CS.UnityEngine.UI.Text as CS.UnityEngine.Component

ImageExt.Sprite = "sprite"

function ImageExt.get:sprite(): CS.UnityEngine.Sprite
    return (self.handler as CS.UnityEngine.UI.Image).sprit
end

function ImageExt.set:sprite(value: CS.UnityEngine.Sprite)
    (self.handler as CS.UnityEngine.UI.Image).sprit = value
end

function ImageExt.get:fillAmount(): number
    return (self.handler as CS.UnityEngine.UI.Image).fillAmount
end

function ImageExt.set:fillAmount(value: number)
    (self.handler as CS.UnityEngine.UI.Image).fillAmount = value
end

function ImageExt.get:GameObject(): GameObjectExtType
    return self.gameObject
end

function ImageExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

return ImageExt