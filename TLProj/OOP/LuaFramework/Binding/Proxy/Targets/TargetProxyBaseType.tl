local Object = require("OOP.ObjectType")

local record Get
    target: Object
    Target: function
end

local record Set
    target: Object
    Target: function
end

local record TargetProxyBase
    get: Get
    Set: Set

    target: Object
    Target: Object

    targetName: string

    ctor: function
    dtor: function
    IsAlive: function(TargetProxyBase, Object)
end

return TargetProxyBase