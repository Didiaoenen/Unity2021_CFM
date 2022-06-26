require("OOP.Class")

local BindGameObject = require("LuaFramework.Views.BindComponents.BindGameObject")
local ObservableObject = require("LuaFramework.Observables.ObservableObject")

local BindButton = class("BindButton", ObservableObject) as BindButtonType

function BindButton.get:OnClick(): function
    return self.onClick
end

function BindButton.set:OnClick(value: function)
    self.onClick = value
end

function BindButton.get:GameObject(): BindGameObjectType
    return self.gameObject
end

function BindButton:ctor()
    ObservableObject.ctor(self)
    self.onClick = self.OnClickEvent
    self.gameObject = BindGameObject.new()
    self.gameObject.ActiveSelf = true
end

function BindButton:Bind(view: WindowViewBaseType, cType: ComponentExtType, name: string, mode: number)
    local component = view:GetComponentWithName(cType, name)
    local bindingSet = view.bindingSet as BindingSetType
    local buttonExt = component as ButtonExtType

    local builder = bindingSet:BindByTarget(component as LuaObject)
    builder:For(buttonExt.OnClick):To(name .. ".OnClick"):SetMode(mode)

    builder = bindingSet:BindByTarget(component.gameObject as LuaObject)
    builder:For(buttonExt.GameObject.ActiveSelf):To(name .. ".GameObject.ActiveSelf"):SetMode(mode)
end

function BindButton:OnClickEvent()
    print("OnClickEvent")
end

return BindButton