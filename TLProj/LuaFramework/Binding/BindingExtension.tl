

require("OOP.Class")

local BindingSet = require("LuaFramework.Binding.Builder.BindingSet")
local BindingContext = require("LuaFramework.Binding.Contexts.BindingContext")

local BindingExtension = class("BindingExtension") as BindingExtensionType

function BindingExtension.get:BindingContext(): BindingContextType
    if nil == self.bindingContext then
        self.bindingContext = BindingContext.new(self.behaviour)
    end
    return self.bindingContext
end

function BindingExtension:ctor(behaviour: ObjectType)
    self.bindingContext = nil
    self.behaviour = behaviour
end

function BindingExtension:CreateBindingSet(): BindingSetType
    local context = self.BindingContext
    return BindingSet.new(context, self.behaviour)
end

function BindingExtension:CreateBindingSetWithContext(dataContext: ObjectType): BindingSetType
    local context = self.BindingContext
    context.DataContext = dataContext
    return BindingSet.new(context, self.behaviour)
end

function BindingExtension:SetDataContext(dataContext: ObjectType)
    self.BindingContext.DataContext = dataContext
end

function BindingExtension:GetDataContext(): ObjectType
    return self.BindingContext.DataContext
end

function BindingExtension:dtor()
end

return BindingExtension