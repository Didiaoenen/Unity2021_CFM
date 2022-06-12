local EventHandler = require("OOP.LuaFramework.EventHandlerType")

local record Set
    valueChanged: {EventHandler}
    ValueChanged: function
end

local record NotifiableSourceProxyBase
    set: Set

    valueChanged: {EventHandler}
    ValueChanged: function

    ctor: function
    dtor: function
    RaiseValueChanged: function(NotifiableSourceProxyBase)
end

return NotifiableSourceProxyBase