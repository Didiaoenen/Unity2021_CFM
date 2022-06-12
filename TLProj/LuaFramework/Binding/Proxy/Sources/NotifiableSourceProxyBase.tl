

require("OOP.Class")

local INotifiable = require("LuaFramework.Binding.Proxy.INotifiable")
local SourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.SourceProxyBase")

local NotifiableSourceProxyBase = class("NotifiableSourceProxyBase", SourceProxyBase, INotifiable) as NotifiableSourceProxyBaseType

function NotifiableSourceProxyBase.set:ValueChanged(value: ObjectType)
    setcallback(self.valueChanged, value)
end

function NotifiableSourceProxyBase:ctor(source: ObjectType)
    SourceProxyBase.ctor(self, source)
    self.valueChanged = {}
end

function NotifiableSourceProxyBase:RaiseValueChanged()
    for _, v in ipairs(self.valueChanged) do
        v.callback(v.this)
    end
end

function NotifiableSourceProxyBase:dtor()
end

return NotifiableSourceProxyBase