require("OOP.Class")

local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")

local BindText = class("BindText", BindComponent) as BindTextType

function BindText.get:Text(): string
    return self.text
end

function BindText.set:Text(value: string)
    self:Set({name = "text"}, value, "Text")
end

function BindText:ctor()
    BindComponent.ctor(self)
    self.text = ""
end

function BindText:BindWithName(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local textExt = component as TextExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(textExt.Text):To(name .. ".Text"):SetMode(mode)

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(textExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(mode)
end

return BindText