local BindComponent = require("LuaFramework.Views.BindComponents.BindComponent")
local ImageExt = require("LuaFramework.Views.Components.ImageExt")

local BindImage = class("BindImage", BindComponent) as BindImageType

function BindImage.get:Sprite(): CS.UnityEngine.Sprite
    return self.sprite
end

function BindImage.set:Sprite(value: CS.UnityEngine.Sprite)
    self:Set({name = "sprite"}, value, "Sprite")
end

function BindImage.get:FillAmount(): number
    return self.fillAmount
end

function BindImage.set:FillAmount(value: number)
    self:Set({name = "fillAmount"}, value, "FillAmount")
end

function BindImage:ctor()
    BindComponent.ctor(self)
end

function BindImage:BindWithName(view: WindowViewBaseType, name: string, modes: {number})
    local component = view:GetComponentWithName(ImageExt as ComponentExtType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local imageExt = component as ImageExtType

    local index: integer = 0
    local _Mode = function(): number
        index = #(modes) > 1 and index + 1 or 1
        return modes[index]
    end

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(imageExt.Sprite):To(name .. ".Sprite"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(imageExt.FillAmount):To(name .. ".FillAmount"):SetMode(_Mode())

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(imageExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(_Mode())
end

function BindImage:dtor()
end

return BindImage