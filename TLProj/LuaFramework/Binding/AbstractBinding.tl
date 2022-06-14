

require("OOP.Class")

local AbstractBinding = class("AbstractBinding") as AbstractBindingType

function AbstractBinding.get:Target(): BaseType
    return self.target
end

function AbstractBinding.get:DataContext(): BaseType
    return self.dataContext
end

function AbstractBinding.set:DataContext(value: BaseType)
    self.dataContext = value
end

function AbstractBinding:ctor(dataContext: BaseType, target: BaseType)
    self.dataContext = dataContext
    self.target = target
end

function AbstractBinding:IsAlive(target: BaseType)
    print(target)
end

function AbstractBinding:OnDataContextChanged()
end

function AbstractBinding:dtor()
end

return AbstractBinding