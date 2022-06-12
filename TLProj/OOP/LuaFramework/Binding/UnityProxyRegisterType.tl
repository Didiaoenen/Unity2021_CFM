local record ProxyFunction
    getter: function
    setter: function
end

local record UnityProxyRegister
    
    registerProxies: {ProxyFunction}

    new: function
    ctor: function
    dtor: function

    __singleton: function(UnityProxyRegister)

    Initialize: function(UnityProxyRegister)
    Get: function(UnityProxyRegister, string): ProxyFunction
end

return UnityProxyRegister