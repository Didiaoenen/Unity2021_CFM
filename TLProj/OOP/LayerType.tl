
global record LayerData
    WindowType: number
    OrderInLayer: number
    OrderStep: number
end

local record Get
    windowType: number
    WindowType: function(Get): number
end

local record Set
    windowType: number
    WindowType: function(Set, number)
end

local record Layer
    get: Get
    set: Set
    WindowType: number

    windowType: number
    orderInLayer: number
    orderStep: number
    maxOrder: number
    minOrder: number

    new: function(Layer): Layer
    ctor: function(Layer, LayerData)
    dtor: function(Layer)
    PushWindowOrder: function(Layer): number
    PopWindowOder: function(Layer)
end

return Layer