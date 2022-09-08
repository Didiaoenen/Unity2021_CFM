local BindingProxyBase = require("LuaFramework.Binding.Proxy.BindingProxyBase")

local SourceProxyBase = class("SourceProxyBase", BindingProxyBase) as SourceProxyBaseType

function SourceProxyBase.get:Source(): LuaObject
    return self.source
end

function SourceProxyBase.set:Source(value: LuaObject)
    self.source = value
end

function SourceProxyBase:ctor(source: LuaObject)
    self.source = source
end

return SourceProxyBase