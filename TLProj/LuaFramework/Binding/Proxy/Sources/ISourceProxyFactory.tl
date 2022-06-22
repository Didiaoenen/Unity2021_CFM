require("OOP.Class")

local ISourceProxyFactory = class("ISourceProxyFactory") as ISourceProxyFactoryType

function ISourceProxyFactory:CreateProxy(source: LuaObject, description: SourceDescriptionType): SourceProxyBaseType
end

return ISourceProxyFactory