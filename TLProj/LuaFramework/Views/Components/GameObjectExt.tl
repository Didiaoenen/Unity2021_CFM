require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local GameObjectExt = class("GameObjectExt", ComponentExt) as GameObjectExtType

GameObjectExt.CSObject = CS.UnityEngine.GameObject as CS.UnityEngine.Component

GameObjectExt.ActiveSelf = "activeSelf"

function GameObjectExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

function GameObjectExt:GetComponent(cType: CS.System.Type): CS.UnityEngine.Component
    return self.handler:GetComponent(cType)
end

function GameObjectExt:AddComponent(cType: CS.System.Type): CS.UnityEngine.Component
    return self.handler:AddComponent(cType)
end

return GameObjectExt