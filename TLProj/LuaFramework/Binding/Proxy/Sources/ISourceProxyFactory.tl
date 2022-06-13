

require("OOP.Class")

local ISourceProxyFactory = class("ISourceProxyFactory") as ISourceProxyFactoryType

function ISourceProxyFactory:CreateProxy(source: ObjectType, description: SourceDescriptionType): SourceProxyBaseType
    print(source, description)
end

return ISourceProxyFactory