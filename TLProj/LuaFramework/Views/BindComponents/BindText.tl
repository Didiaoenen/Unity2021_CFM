local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
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
    self.text = ""
    self.color = CS.UnityEngine.Color.white
end

function BindText:BindWithName(view: ViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(TextExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local textExt = component as TextExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(textExt.Text):To(name .. ".Text"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(textExt.Color):To(name .. ".Color"):SetMode(_Mode())

    self.gameObject:BindToTarget(bindingSet, name .. ".GameObject.ActiveSelf", textExt.gameObject as ComponentExtType, _Mode())
end

function BindText:dtor()
end

return BindText