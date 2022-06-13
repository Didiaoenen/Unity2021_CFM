

require("OOP.Class")

local ITargetProxyFactory = class("ITargetProxyFactory") as ITargetProxyFactoryType

function ITargetProxyFactory:CreateProxy(target: ObjectType, discription: BindingDescriptionType): TargetProxyBaseType
    print(target, discription)
end

return ITargetProxyFactory