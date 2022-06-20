

require("OOP.Class")

local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local MonoVariableArrayExt = class("MonoVariableArrayExt", ComponentExt) as MonoVariableArrayExtType

MonoVariableArrayExt.CSObject = CS.CSObject.Simple.MonoVariableArray as Component

function MonoVariableArrayExt:ctor(handler)
    ComponentExt.ctor(self, handler)
end

function MonoVariableArrayExt:Get(name: string): Component
    return (self.handler as MonoVariableArray).variableArray:Get(name)
end

return MonoVariableArrayExt