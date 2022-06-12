local Object = require("OOP.ObjectType")

local record Get
    raised: function
    Raised: function
end

local record Set
    raised: function
    Raised: function
end

local record InteractionRequest
    get: Get
    set: Set
    
    raised: function
    Raised: function

    sender: Object

    ctor: function
    dtor: function
    Raise: function(InteractionRequest, Object, function)
end

return InteractionRequest