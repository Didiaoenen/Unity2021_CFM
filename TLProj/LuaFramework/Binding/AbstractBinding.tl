

require("OOP.Class")

local AbstractBinding = class("AbstractBinding") as AbstractBindingType

function AbstractBinding.get:BindingContext(): Object
    return self.bindingContext
end

function AbstractBinding.set:BindingContext(value: Object)
    self.bindingContext = value
end

function AbstractBinding.get:Target(): Object
    return self.target
end

function AbstractBinding.get:DataContext(): Object
    return self.dataContext
end

function AbstractBinding.set:DataContext(value: Object)
    self.dataContext = value
end

function AbstractBinding:ctor(bindingContext: Object, dataContext: Object, target: Object)
    self.bindingContext = bindingContext
    self.dataContext = dataContext
    self.target = target
end

function AbstractBinding:IsAlive(target: Object)
end

function AbstractBinding:OnDataContextChanged()
end

function AbstractBinding:dtor()
end

return AbstractBinding