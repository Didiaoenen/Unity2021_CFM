local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local MonoVariableArrayExt = class("MonoVariableArrayExt", ComponentExt) as MonoVariableArrayExtType

MonoVariableArrayExt.CSObject = CS.Simple.MonoVariableArray as CS.UnityEngine.Component

function MonoVariableArrayExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

function MonoVariableArrayExt:Get(name: string): CS.UnityEngine.Component
    return (self.handler as CS.Simple.MonoVariableArray).variableArray:Get(name)
end

function MonoVariableArrayExt:dtor()
end

return MonoVariableArrayExt