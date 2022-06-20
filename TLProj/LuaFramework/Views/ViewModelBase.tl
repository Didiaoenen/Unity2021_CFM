

require("OOP.Class")

local ObservableObject = require("LuaFramework.Observables.ObservableObject")
local RectTransformExt = require("LuaFramework.Views.Components.RectTransformExt")

local ViewModelBase = class("ViewModelBase", ObservableObject) as ViewModelBaseType

function ViewModelBase.get:Parent(): TransformExtType
    return self.parent
end

function ViewModelBase.set:Parent(value: TransformExtType)
    self:Set({name = "parent"}, value, "Parent")
end

function ViewModelBase.get:LocalRotation(): Quaternion
    return self.localRotation
end

function ViewModelBase.set:LocalRotation(value: Quaternion)
    self:Set({name = "localRotation"}, value, "LocalRotation")
end

function ViewModelBase.get:LocalPosition(): Vector3
    return self.localPosition
end

function ViewModelBase.set:LocalPosition(value: Vector3)
    self:Set({name = "localPosition"}, value, "LocalPosition")
end

function ViewModelBase.get:LocalScale(): Vector3
    return self.localScale
end

function ViewModelBase.set:LocalScale(value: Vector3)
    self:Set({name = "localScale"}, value, "LocalScale")
end

function ViewModelBase.get:OffsetMax(): Vector2
    return self.offsetMax
end

function ViewModelBase.set:OffsetMax(value: Vector2)
    self:Set({name = "offsetMax"}, value, "OffsetMax")
end

function ViewModelBase.get:OffsetMin(): Vector2
    return self.offsetMin
end

function ViewModelBase.set:OffsetMin(value: Vector2)
    self:Set({name = "offsetMin"}, value, "OffsetMin")
end

function ViewModelBase.get:ActiveSelf(): boolean
    return self.activeSelf
end

function ViewModelBase.set:ActiveSelf(value: boolean)
    self:Set({name = "activeSelf"}, value, "ActiveSelf")
end

function ViewModelBase.get:OverrideSorting(): boolean
    return self.overrideSorting
end

function ViewModelBase.set:OverrideSorting(value: boolean)
    self:Set({name = "overrideSorting"}, value, "OverrideSorting")
end

function ViewModelBase.get:SortingOrder(): number
    return self.sortingOrder
end

function ViewModelBase.set:SortingOrder(value: number)
    self:Set({name = "sortingOrder"}, value, "SortingOrder")
end

function ViewModelBase.get:Alpha(): number 
    return self.alpha
end

function ViewModelBase.set:Alpha(value: number)
    self:Set({name = "alpha"}, value, "Alpha")
end

function ViewModelBase:ctor(messenger: MessengerType)
    ObservableObject.ctor(self)
    self.messenger = messenger

    self.parent = RectTransformExt.new(CS.CSObject.GameObject.Find("UIRoot").transform) as TransformExtType
    self.localRotation = CS.CSObject.Quaternion.identity
    self.localPosition = CS.CSObject.Vector3.zero
    self.localScale = CS.CSObject.Vector3.one
    self.offsetMax = CS.CSObject.Vector2.zero
    self.offsetMin = CS.CSObject.Vector2.zero
    self.activeSelf = true
    self.overrideSorting = true
    self.sortingOrder = 0
    self.alpha = 1
end

function ViewModelBase:SetWithBroadCast(field: SetType, newValue: any, propertyName: string, broadcast: boolean)
    if field ~= newValue then
        ObservableObject.Set(self, field, newValue, propertyName)

        if broadcast then
            local oldValue = Functions.ClassGet(field as BaseType, field.name)
            self:Broadcast(oldValue, newValue, propertyName)
        end
    end
end

function ViewModelBase:Broadcast(oldValue: any, newVlaue: any, propertyName: string)
    print(oldValue, newVlaue, propertyName)
end

function ViewModelBase:dtor()
end

return ViewModelBase