require("OOP.Class")

local INodeProxyFactory = class("INodeProxyFactory") as INodeProxyFactoryType

function INodeProxyFactory:Create(source: LuaObject, path: {string}): SourceProxyBaseType
end

return INodeProxyFactory