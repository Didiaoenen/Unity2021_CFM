require("OOP.Class")

local BindGameObject = class("BindGameObject") as BindGameObjectType

function BindGameObject.get:ActiveSelf(): boolean
    return self.activeSelf
end

function BindGameObject.set:ActiveSelf(value: boolean)
    self.activeSelf = value
end

function BindGameObject:ctor()
    self.activeSelf = false
end

return BindGameObject