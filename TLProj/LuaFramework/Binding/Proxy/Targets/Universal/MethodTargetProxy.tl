

require("OOP.Class")

local TargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.TargetProxyBase")

local MethodTargetProxy = class("MethodTargetProxy") as MethodTargetProxyType

function MethodTargetProxy.get:DefaultMode(): number
    return BindingMode.OneWayToSource
end

function MethodTargetProxy.get:ProxyMethodInfo(): string
    return self.methodInfo
end

function MethodTargetProxy:ctor(target: ObjectType, methodInfo: string)
    TargetProxyBase.ctor(self, target)
    self.methodInfo = methodInfo
    self.invoker = self
end

function MethodTargetProxy:GetValue(): any
    return self.invoker
end

function MethodTargetProxy:Invoke(sender: ObjectType, ...: any)
    local target = self.target
    if nil == target then
        return
    end

    local functionType = Functions.ClassGet(target, self.methodInfo) as function
    functionType(target, sender, ...)
end

function MethodTargetProxy:dtor()
    
end

return MethodTargetProxy