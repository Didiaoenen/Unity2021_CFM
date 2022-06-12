local Object = require("OOP.ObjectType")

local record Get
    source: Object
    Source: function
end

local record Set
    source: Object
    Source: function
end

local record SourceProxyBase
    get: Get
    set: Set

    source: Object

    ctor: function
    dtor: function
end

return SourceProxyBase