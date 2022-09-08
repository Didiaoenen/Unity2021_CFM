local ObservableObject = require("LuaFramework.Observables.ObservableObject")

local BindGameObject = class("BindGameObject", ObservableObject) as BindGameObjectType

function BindGameObject.get:ActiveSelf(): boolean
    return self.activeSelf
end

function BindGameObject.set:ActiveSelf(value: boolean)
    self:Set({name = "activeSelf"}, value, "ActiveSelf")
end

function BindGameObject:ctor()
    ObservableObject.ctor(self)
    self.activeSelf = true
end

return BindGameObject