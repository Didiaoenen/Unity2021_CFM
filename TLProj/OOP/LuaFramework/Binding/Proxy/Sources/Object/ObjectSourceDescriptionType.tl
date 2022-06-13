local record Get
    path: {string}
    Path: function
end

local record Set
    path: {string}
    Path: function
end

local record ObjectSourceDescription
    get: Get
    set: Set

    path: {string}
    Path: {string}
    isStatic: boolean

    new: function(...: any): ObjectSourceDescription
    ctor: function
    dtor: function
end

return ObjectSourceDescription