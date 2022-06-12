local record Get
    PropertyName: function
end

local record PropertyChangedEventArgs
    get: Get

    PropertyName: string
end

return PropertyChangedEventArgs