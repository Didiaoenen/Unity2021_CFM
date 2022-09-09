local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")
local ObservableObject = require("LuaFramework.Observables.ObservableObject")

local BindComponent = class("BindComponent", ObservableObject) as BindComponentType

function BindComponent.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindComponent:ctor()
    ObservableObject.ctor(self)
    self.gameObject = BindGameObject.new()
    self.gameObject.ActiveSelf = true
end

function BindComponent:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, modes: {number})
    assert({view, cType, name, modes}, "")
end

function BindComponent:dtor()
end

return BindComponent