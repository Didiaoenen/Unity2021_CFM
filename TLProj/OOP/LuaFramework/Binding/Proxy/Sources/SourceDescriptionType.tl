local record Get
    isStatic: boolean
    IsStatic: function
end

local record Set
    isStatic: boolean
    IsStatic: function
end

local record SourceDescription
    get: Get
    set: Set

    isStatic: boolean
    IsStatic: function(SourceDescription): boolean

    ctor: function(SourceDescription)
    dtor: function(SourceDescription)
end

return SourceDescription