

require("OOP.Class")

local INodeProxyFactory = class("INodeProxyFactory") as INodeProxyFactoryType

function INodeProxyFactory:Create(source: BaseType, path: {string}): SourceProxyBaseType
    print(source, path)
end

return INodeProxyFactory