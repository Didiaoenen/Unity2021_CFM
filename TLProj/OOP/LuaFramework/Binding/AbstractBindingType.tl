local Object = require("OOP.ObjectType")

local record Get
    dataContext: Object
    DataContext: function

    target: Object
    Target: function
end

local record Set
    dataContext: Object
    DataContext: function
end

local record AbstractBinding
    get: Get
    set: Set

    dataContext: Object
    DataContext: Object
    target: Object
    Target: Object

    ctor: function
    dtor: function

    IsAlive: function(AbstractBinding, Object)
    OnDataContextChanged: function(AbstractBinding)
end

return AbstractBinding