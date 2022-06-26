require("OOP.Class")

local GameObjectExt = require("LuaFramework.Views.Components.GameObjectExt")

local ComponentExt = class("ComponentExt") as ComponentExtType

function ComponentExt:ctor(handler: CS.UnityEngine.Component)
    self.handler = handler
    self.gameObject = GameObjectExt.new(handler.gameObject)
end

function ComponentExt:dtor()
    self.handler = nil
end

return ComponentExt