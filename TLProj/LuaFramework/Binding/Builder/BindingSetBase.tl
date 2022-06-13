

require("OOP.Class")

local BindingSetBase = class("BindingSetBase") as BindingSetBaseType

function BindingSetBase:ctor(context: BindingContextType)
    self.context = context
    self.builders = {}
end

function BindingSetBase:Build()
    for _, v in ipairs(self.builders) do
        v:Build()
    end
end

function BindingSetBase:dtor()
end

return BindingSetBase