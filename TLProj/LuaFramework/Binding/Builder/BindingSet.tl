

require("OOP.Class")

local BindingSetBase = require("LuaFramework.Binding.Builder.BindingSetBase")
local BindingBuilder = require("LuaFramework.Binding.Builder.BindingBuilder")

local BindingSet = class("BindingSet") as BindingSetType

function BindingSet:ctor(context: BindingContextType, target: ObjectType)
    BindingSetBase.ctor(self, context)
    self.target = target
end

function BindingSet:Bind(): BindingBuilderBaseType
    local builder = BindingBuilder.new(self.context, self.target) as BindingBuilderBaseType
    table.insert(self.builders, builder)
    return builder
end

function BindingSet:BindByTarget(target: ObjectType): BindingBuilderBaseType
    local builder = BindingBuilder.new(self.context, target) as BindingBuilderBaseType
    table.insert(self.builders, builder)
    return builder
end

function BindingSet:dtor()
end

return BindingSet