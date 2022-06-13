local BindingContext = require("OOP.LuaFramework.Binding.Contexts.BindingContextType")
local BindingBuilderBase = require("OOP.LuaFramework.Binding.Builder.BindingBuilderBaseType")

local record BindingSetBase
    context: BindingContext
    builders: {BindingBuilderBase}

    ctor: function
    dtor: function
    Build: function(BindingSetBase)
end

return BindingSetBase