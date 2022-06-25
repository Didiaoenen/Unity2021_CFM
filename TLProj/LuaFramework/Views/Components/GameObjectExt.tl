require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local GameObjectExt = class("GameObjectExt", ComponentExt) as GameObjectExtType

GameObjectExt.CSObject = CS.UnityEngine.GameObject as CS.UnityEngine.Component

GameObjectExt.ActiveSelf = "activeSelf"

function GameObjectExt.get:activeSelf(): boolean
    return (self.handler as CS.UnityEngine.GameObject).activeSelf
end

function GameObjectExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

function GameObjectExt:GetComponent(cType: CS.System.Type): CS.UnityEngine.Component
    return self.handler:GetComponent(cType)
end

function GameObjectExt:AddComponent(cType: CS.System.Type): CS.UnityEngine.Component
    return self.handler:AddComponent(cType)
end

function GameObjectExt:SetActive(value: boolean)
    (self.handler as CS.UnityEngine.GameObject):SetActive(value)
end

return GameObjectExt