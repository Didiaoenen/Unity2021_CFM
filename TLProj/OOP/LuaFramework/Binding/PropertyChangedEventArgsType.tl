local record Get
    propertyName: string
    PropertyName: function
end

local record PropertyChangedEventArgs
    get: Get
    
    propertyName: string
    PropertyName: string

    ctor: function
end

return PropertyChangedEventArgs