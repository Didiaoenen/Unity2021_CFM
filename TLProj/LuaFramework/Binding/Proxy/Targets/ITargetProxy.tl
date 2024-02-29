local ITargetProxy = class("ITargetProxy") as ITargetProxyType

function ITargetProxy.get:Target(): any
    return nil
end

function ITargetProxy.get:DefaultMode(): number
    return 0
end

return ITargetProxy