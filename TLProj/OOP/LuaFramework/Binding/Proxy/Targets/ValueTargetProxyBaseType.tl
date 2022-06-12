local Object = require("OOP.ObjectType")
local EventHandler = require("OOP.LuaFramework.EventHandlerType")

local record Set
    subScribed: boolean

    valueChanged: {EventHandler}
    ValueChanged: function

    Subscribe: function(Set)
end

local record ValueTargetProxyBase
    set: Set

    subScribed: boolean

    valueChanged: {EventHandler}
    ValueChanged: function

    target: Object

    ctor: function
    dtor: function
    Subscribe: function(ValueTargetProxyBase)
    Unsubscribe: function(ValueTargetProxyBase)
    DoSubscribeForValueChange: function(ValueTargetProxyBase, Object)
    DoUnsubscribeForValueChange: function(ValueTargetProxyBase, Object)
    RaiseValueChanged: function(ValueTargetProxyBase)
    GetValue: function(ValueTargetProxyBase): any
    SetValue: function(ValueTargetProxyBase, any)
end

return ValueTargetProxyBase