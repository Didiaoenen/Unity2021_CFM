local Object = require("OOP.ObjectType")
local PropertyChangedEventArgs = require("OOP.LuaFramework.Binding.PropertyChangedEventArgsType")

local record EventHandler
    this: Object
    callback: function
    e: PropertyChangedEventArgs
end

return EventHandler