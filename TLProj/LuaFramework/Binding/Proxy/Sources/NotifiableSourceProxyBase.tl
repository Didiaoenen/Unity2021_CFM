

require("OOP.Class")

local SourceProxyBase = require("LuaFramework.Binding.Proxy.Sources.SourceProxyBase")
local INotifiable = require("LuaFramework.Binding.Proxy.INotifiable")
local TableExtention = require("LuaFramework.Common.TableExtention")

local NotifiableSourceProxyBase = class("NotifiableSourceProxyBase", SourceProxyBase, INotifiable) as NotifiableSourceProxyBaseType

function NotifiableSourceProxyBase.set:ValueChanged(value: BaseType)
    setcallback(self.valueChanged, value)
end

function NotifiableSourceProxyBase:ctor(source: BaseType)
    SourceProxyBase.ctor(self, source)
    self.valueChanged = {}
end

function NotifiableSourceProxyBase:RaiseValueChanged()
    TableExtention.Foreach(self.valueChanged, function(i: number, v: EventHandlerType)
        v.callback(v.this)
    end)
end

function NotifiableSourceProxyBase:dtor()
end

return NotifiableSourceProxyBase