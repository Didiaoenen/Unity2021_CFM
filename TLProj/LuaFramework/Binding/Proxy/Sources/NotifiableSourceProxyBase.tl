

require("OOP.Class")

local SourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.SourceProxyBase")
local INotifiable = require("LuaFramework.Binding.Proxy.INotifiable")

local NotifiableSourceProxyBase = class("NotifiableSourceProxyBase", SourceProxyBase, INotifiable) as NotifiableSourceProxyBaseType

function NotifiableSourceProxyBase.set:ValueChanged(value: BaseType)
    setcallback(self.valueChanged, value)
end

function NotifiableSourceProxyBase:ctor(source: BaseType)
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