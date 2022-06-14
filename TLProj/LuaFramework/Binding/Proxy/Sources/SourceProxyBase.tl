

require("OOP.Class")

local BindingProxyBase = require("LuaFramework.Binding.Proxy.BindingProxyBase")

local SourceProxyBase = class("SourceProxyBase", BindingProxyBase) as SourceProxyBaseType

function SourceProxyBase.get:Source(): BaseType
    return self.source
end

function SourceProxyBase.set:Source(value: BaseType)
    self.source = value
end

function SourceProxyBase:ctor(source: BaseType)
    self.source = source
end

return SourceProxyBase