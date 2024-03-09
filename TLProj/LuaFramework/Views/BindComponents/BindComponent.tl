local ObservableObject = require("LuaFramework.Observables.ObservableObject")

local BindComponent = class("BindComponent", ObservableObject) as BindComponentType

function BindComponent.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindComponent:ctor()
    ObservableObject.ctor(self)
    self.componentExt = nil
end

function BindComponent:BindWithName(view: ViewBaseType, name: string, modes: {number})
    assert({view, name, modes}, "")
end

function BindComponent:dtor()
end

return BindComponent