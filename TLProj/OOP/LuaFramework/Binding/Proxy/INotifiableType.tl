local EventHandler = require("OOP.LuaFramework.EventHandlerType")

local record Set
    valueChanged: EventHandler
    ValueChanged: function
end

local record INotifiable
    set: Set

    valueChanged: {EventHandler}
    ValueChanged: EventHandler
end

return INotifiable