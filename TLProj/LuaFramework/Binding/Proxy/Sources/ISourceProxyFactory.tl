

require("OOP.Class")

local ISourceProxyFactory = class("ISourceProxyFactory") as ISourceProxyFactoryType

function ISourceProxyFactory:CreateProxy(source: BaseType, description: SourceDescriptionType): SourceProxyBaseType
    print(source, description)
end

return ISourceProxyFactory