local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")
local TextExt = require("LuaFramework.Views.Components.TextExt")

local BindText = class("BindText", BindComponent) as BindTextType

function BindText.get:Text(): string
    return self.text
end

function BindText.set:Text(value: string)
    self:Set({name = "text"}, value, "Text")
end

function BindText.get:Color(): CS.UnityEngine.Color
    return self.color
end

function BindText.set:Color(value: CS.UnityEngine.Color)
    self:Set({name = "color"}, value, "Color")
end

function BindText:ctor()
    BindComponent.ctor(self)
    self.gameObject = BindGameObject.new()
    self.text = ""
    self.color = CS.UnityEngine.Color.white
end

function BindText:BindWithName(view: ViewBaseType, name: string, modes: {number})
    self.componentExt = view:GetComponentWithName(TextExt as ComponentExtType, name) as TextExtType
    local bindingSet = view.bindingSet as BindingSetType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.Text):To(name .. ".Text"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt as LuaObject)
    builder:For(self.componentExt.Color):To(name .. ".Color"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(self.componentExt.gameObject as LuaObject)
    builder:For(self.componentExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

function BindText:dtor()
end

return BindText