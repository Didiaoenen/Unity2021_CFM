local Object = require("OOP.ObjectType")

local record Get
    target: Object
    Target: function

    DefaultMode: function
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

    DefaultMode: number

    targetName: string

    ctor: function
    delete: function
    dtor: function

    IsAlive: function(TargetProxyBase, Object): boolean
end

return TargetProxyBase