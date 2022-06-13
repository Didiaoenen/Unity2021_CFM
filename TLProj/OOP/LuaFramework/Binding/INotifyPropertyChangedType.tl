local EventHandler = require("OOP.LuaFramework.EventHandlerType")

local record Set
    propertyChanged: EventHandler
    PropertyChanged: function
end

local record INotifyPropertyChanged
    set: Set

    propertyChanged: EventHandler
    PropertyChanged: EventHandler
end

return INotifyPropertyChanged