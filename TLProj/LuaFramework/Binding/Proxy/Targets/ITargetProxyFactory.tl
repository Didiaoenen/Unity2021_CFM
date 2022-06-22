require("OOP.Class")

local ITargetProxyFactory = class("ITargetProxyFactory") as ITargetProxyFactoryType

function ITargetProxyFactory:CreateProxy(target: LuaObject, discription: BindingDescriptionType): TargetProxyBaseType
end

return ITargetProxyFactory