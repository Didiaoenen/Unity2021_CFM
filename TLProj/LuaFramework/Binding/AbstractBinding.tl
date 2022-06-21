

require("OOP.Class")

local AbstractBinding = class("AbstractBinding") as AbstractBindingType

function AbstractBinding.get:Target(): BaseType
    assert(self.target, "")
    local target = (self.target as TargetProxyBaseType).Target
    return self:IsAlive(target) and target
end

function AbstractBinding.get:DataContext(): BaseType
    return self.dataContext
end

function AbstractBinding.set:DataContext(value: BaseType)
    if value ~= self.dataContext then
        self.dataContext = value
        self:OnDataContextChanged()
    end
end

function AbstractBinding:ctor(dataContext: BaseType, target: BaseType)
    self.dataContext = dataContext
    self.target = target
end

function AbstractBinding:IsAlive(target: BaseType): boolean
    print(target)
    return true
end

function AbstractBinding:OnDataContextChanged()
end

function AbstractBinding:dtor()
end

return AbstractBinding