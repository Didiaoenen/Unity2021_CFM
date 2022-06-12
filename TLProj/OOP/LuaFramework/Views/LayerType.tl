
global record LayerData
    WindowType: number
    OrderInLayer: number
    OrderStep: number
end

local record Get
    windowType: number
    WindowType: function
end

local record Set
    windowType: number
    WindowType: function
end

local record Layer
    get: Get
    set: Set
    
    windowType: number
    WindowType: number

    orderInLayer: number
    orderStep: number
    maxOrder: number
    minOrder: number

    new: function
    ctor: function
    dtor: function
    PushWindowOrder: function(Layer): number
    PopWindowOder: function(Layer)
end

return Layer