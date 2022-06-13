local Object = require("OOP.ObjectType")

local record Get
    DefaultMode: function
end

local record UnityPropertyProxy
    get: Get

    DefaultMode: number

    target: Object
    unityEvent: string

    new: function(...: any): UnityPropertyProxy
    ctor: function
    dtor: function
    DoSubscribeForValueChange: function(UnityPropertyProxy, Object)
    DoUnsubscribeForValueChange: function(UnityPropertyProxy)
    OnValueChanged: function(UnityPropertyProxy, ...: any)
    RaiseValueChanged: function(UnityPropertyProxy, ...: any)
end

return UnityPropertyProxy