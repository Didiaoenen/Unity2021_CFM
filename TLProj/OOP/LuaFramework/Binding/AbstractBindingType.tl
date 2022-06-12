local Object = require("OOP.ObjectType")

local record Get
    bindingContext: Object
    BindingContext: function
    
    dataContext: Object
    DataContext: function

    target: Object
    Target: function
end

local record Set
    bindingContext: Object
    BindingContext: function

    dataContext: Object
    DataContext: function
end

local record AbstractBinding
    get: Get
    set: Set

    bindingContext: Object
    dataContext: Object
    target: Object

    ctor: function
    dtor: function

    IsAlive: function
    OnDataContextChanged: function
end

return AbstractBinding