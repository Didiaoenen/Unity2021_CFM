local Object = require("OOP.ObjectType")

local record Get
    context: Object
    Context: function

    callback: function
    Callback: function
end

local record InteractionEventArgs
    get: Get

    context: Object
    Context: function
    
    callback: function
    Callback: function

    new: function
    ctor: function
    dtor: function
end

return InteractionEventArgs