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

function BindGameObject:BindWithName(view: WindowViewBaseType, cType: GameObjectExtType, name: string, modes: {number})
    local gameObject = view:GetGameObjectWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end
    
    local builder = bindingSet:BindByTarget(gameObject as LuaObject)
    builder:For(gameObject.ActiveSelf):To(name .. ".ActiveSelf"):SetMode(_Mode())
end

function BindGameObject:dtor()
end

return BindGameObject