require("OOP.Class")

local INodeProxyFactory = class("INodeProxyFactory") as INodeProxyFactoryType

function INodeProxyFactory:Create(source: LuaObject, token: PathTokenType): SourceProxyBaseType
    assert({source, token}, "")
end

return INodeProxyFactory