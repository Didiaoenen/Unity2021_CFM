require("OOP.Class")

local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")
local ObservableObject = require("LuaFramework.Observables.ObservableObject")

local BindText = class("BindText", ObservableObject) as BindTextType

function BindText.get:Text(): string
    return self.text
end

function BindText.set:Text(value: string)
    self:Set({name = "text"}, value, "Text")
end

function BindText.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindText:ctor()
    ObservableObject.ctor(self)
    self.text = ""
    self.gameObject = BindGameObject.new()
    self.gameObject.ActiveSelf = true
end

function BindText:Bind(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local textExt = component as TextExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(textExt.Text):To(name .. ".Text"):SetMode(mode)

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(textExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(mode)
end

return BindText