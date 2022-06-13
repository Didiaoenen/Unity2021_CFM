

require("OOP.Class")

local TargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.TargetProxyBase")

local EventTargetProxyBase = class("EventTargetProxyBase", TargetProxyBase) as EventTargetProxyBaseType

function EventTargetProxyBase:ctor(target: ObjectType)
    TargetProxyBase.ctor(self, target)
end

function EventTargetProxyBase:SetValue(value: any)
    print(value)
end

function EventTargetProxyBase:dtor()
end

return EventTargetProxyBase