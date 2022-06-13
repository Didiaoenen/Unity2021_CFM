

require("OOP.Class")

local AbstractBinding = class("AbstractBinding") as AbstractBindingType

function AbstractBinding.get:Target(): ObjectType
    return self.target
end

function AbstractBinding.get:DataContext(): ObjectType
    return self.dataContext
end

function AbstractBinding.set:DataContext(value: ObjectType)
    self.dataContext = value
end

function AbstractBinding:ctor(dataContext: ObjectType, target: ObjectType)
    self.dataContext = dataContext
    self.target = target
end

function AbstractBinding:IsAlive(target: ObjectType)
    print(target)
end

function AbstractBinding:OnDataContextChanged()
end

function AbstractBinding:dtor()
end

return AbstractBinding