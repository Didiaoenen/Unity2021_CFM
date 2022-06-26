require("OOP.Class")

local GameObjectExt = class("GameObjectExt") as GameObjectExtType

GameObjectExt.CSObject = CS.UnityEngine.GameObject

GameObjectExt.ActiveSelf = "activeSelf"

function GameObjectExt.get:activeSelf(): boolean
    return self.handler.activeSelf
end

function GameObjectExt:ctor(handler: CS.UnityEngine.GameObject)
    self.handler = handler
end

function GameObjectExt:GetComponent(cType: CS.System.Type): CS.UnityEngine.Component
    return self.handler:GetComponent(cType)
end

function GameObjectExt:AddComponent(cType: CS.System.Type): CS.UnityEngine.Component
    return self.handler:AddComponent(cType)
end

function GameObjectExt:SetActive(value: boolean)
    self.handler:SetActive(value)
end

return GameObjectExt