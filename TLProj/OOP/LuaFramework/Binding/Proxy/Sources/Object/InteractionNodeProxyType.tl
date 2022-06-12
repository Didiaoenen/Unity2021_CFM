local Object = require("OOP.ObjectType")

local record InteractionNodeProxy
    source: Object
    request: string
    invoker: Object

    new: function
    ctor: function
    dtor: function
    BindEvent: function(InteractionNodeProxy)
    UnbindEvent: function(InteractionNodeProxy)
    OnRaised: function(InteractionNodeProxy, Object, ...: any)
    SetValue: function(InteractionNodeProxy, any)
end

return InteractionNodeProxy