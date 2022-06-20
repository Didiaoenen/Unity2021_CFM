

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local GameObjectExt = class("GameObjectExt", ComponentExt) as GameObjectExtType

GameObjectExt.CSObject = CS.CSObject.GameObject as Component

GameObjectExt.ActiveSelf = "activeSelf"

function GameObjectExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

function GameObjectExt:GetComponent(cType: Type): Component
    return self.handler:GetComponent(cType)
end

function GameObjectExt:AddComponent(cType: Type): Component
    return self.handler:AddComponent(cType)
end

return GameObjectExt