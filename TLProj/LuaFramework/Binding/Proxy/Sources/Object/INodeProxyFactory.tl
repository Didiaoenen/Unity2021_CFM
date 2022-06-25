require("OOP.Class")

local INodeProxyFactory = class("INodeProxyFactory") as INodeProxyFactoryType

function INodeProxyFactory:Create(source: LuaObject, token: PathTokenType): SourceProxyBaseType
end

return INodeProxyFactory