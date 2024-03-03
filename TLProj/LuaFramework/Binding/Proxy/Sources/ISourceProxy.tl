local ISourceProxy = class("ISourceProxy") as ISourceProxyType

function ISourceProxy.ctor()
end

function ISourceProxy.get:Source(): any
    return nil
end

return ISourceProxy