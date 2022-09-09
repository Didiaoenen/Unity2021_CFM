local TableExtention = require("LuaFramework.Common.TableExtention")

local BindingSetBase = class("BindingSetBase") as BindingSetBaseType

function BindingSetBase:ctor(context: BindingContextType)
    self.context = context
    self.builders = {}
end

function BindingSetBase:Build()
    TableExtention.Foreach(self.builders, function(_: number, v: BindingBuilderBaseType)
        v:Build()
    end)
end

function BindingSetBase:dtor()
end

return BindingSetBase