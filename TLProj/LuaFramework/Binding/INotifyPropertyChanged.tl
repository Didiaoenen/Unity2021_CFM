

require("OOP.Class")

local INotifyPropertyChanged = class("INotifyPropertyChanged") as INotifyPropertyChangedType

function INotifyPropertyChanged.set:PropertyChanged(value: EventHandlerType)
    self.propertyChanged = value
end

return INotifyPropertyChanged