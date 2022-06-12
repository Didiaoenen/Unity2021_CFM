

require("OOP.Class")

local BindingProxyBase = require("LuaFramework.Binding.Proxy.BindingProxyBase")

local SourceProxyBase = class("SourceProxyBase", BindingProxyBase) as SourceProxyBaseType

function SourceProxyBase.get:Source(): ObjectType
    return self.source
end

function SourceProxyBase.set:Source(value: ObjectType)
    self.source = value
end

function SourceProxyBase:ctor(source: ObjectType)
    self.source = source
end

return SourceProxyBase