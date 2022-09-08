local INotifyPropertyChanged = class("INotifyPropertyChanged") as INotifyPropertyChangedType

function INotifyPropertyChanged.set:PropertyChanged(value: EventHandlerType)
    setcallback(self.propertyChanged, value)
end

function INotifyPropertyChanged:ctor()
    self.propertyChanged = {}
end

return INotifyPropertyChanged