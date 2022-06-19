

require("OOP.Class")

local TableExtention: TableExtentionType

local INotifyPropertyChanged = require("LuaFramework.Binding.INotifyPropertyChanged")
local PropertyChangedEventArgs = require("LuaFramework.Binding.PropertyChangedEventArgs")

local ObservableObject = class("ObservableObject") as ObservableObjectType

function ObservableObject:ctor()
    INotifyPropertyChanged.ctor(self)
end

function ObservableObject:RaisePropertyChanged(property: string)
    self:RaisePropertyChangedWithParam(PropertyChangedEventArgs.new(property))
end

function ObservableObject:RaisePropertyChangedWithParam(param: PropertyChangedEventArgsType)
    TableExtention.Foreach(self.propertyChanged, function (i: number, v: EventHandlerType)
        v.callback(v.this, self, param)
    end)
end

function ObservableObject:Set(field: SetType, newValue: any, property: string, broadcast: boolean)
    local oldValue = Functions.ClassGet(field as BaseType, field.name)
    if oldValue ~= newValue then
        Functions.ClassSet(self, field.name, newValue)
        self:RaisePropertyChanged(property)
    end

    if broadcast then
    end 
end

function ObservableObject:dtor()
    
end

return ObservableObject