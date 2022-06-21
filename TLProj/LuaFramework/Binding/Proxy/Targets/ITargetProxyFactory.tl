

require("OOP.Class")

local ITargetProxyFactory = class("ITargetProxyFactory") as ITargetProxyFactoryType

function ITargetProxyFactory:CreateProxy(target: LuaObject, discription: BindingDescriptionType): TargetProxyBaseType
    print(target, discription)
end

return ITargetProxyFactory