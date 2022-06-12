

require("OOP.Class")

local EventTargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.Universal.EventTargetProxyBase")

local UnityEventProxyBase = class("UnityEventProxyBase", EventTargetProxyBase) as UnityEventProxyBaseType

function UnityEventProxyBase.get:DefaultMode(): number
    return BindingMode.OneWay
end

function UnityEventProxyBase:ctor(target: ObjectType, unityEvent: string)
    EventTargetProxyBase.ctor(self, target)

    self.command = nil
    self.invoker = nil
    self.interactable = nil
    self.unityEvent = unityEvent
end

function UnityEventProxyBase:BindEvent()
end

function UnityEventProxyBase:UnbindEvent()
end

function UnityEventProxyBase:IsValidHandler(handler: any)
end

function  UnityEventProxyBase:IsValidInvoker(invoker: any)
end

function UnityEventProxyBase:SetValue(value: any)
    local target = self.target
    if nil == target then
        return
    end

    if self.command then
        self:UnbindCommand(self.command)
        self.command = nil
    end

    if self.invoker then
        self.invoker = nil
    end

    if nil == value then
        return
    end

    if "userdata" == type(value) then
    
    else
        if type(value) == "table" then
            if (value as ObjectType).is(ProxyInvokerType) then
                self.invoker = value as ProxyInvokerType
            elseif (value as ObjectType).is(SimpleCommandType) then
                if nil == self.interactable or self.interactable == "" then
                    self.interactable = "interactable"
                end

                self.command = value as SimpleCommandType
                self:BindCommand(self.command)
                self:UpdateTargetInteractable()
            end
        end
    end
end

function UnityEventProxyBase:OnExecuteChanged(sender: ObjectType, ...: any)
    Executors:RunOnMainThread(function ()
        self:UpdateTargetInteractable()
    end)
end

function UnityEventProxyBase:UpdateTargetInteractable()
    local target = self.target
    if nil == self.interactable or self.interactable == "" or nil == target then
        return
    end

    local value = nil == self.command and false or self.command:CanExecute()
    Functions.ClassSet(target, self.interactable, value)
end

function UnityEventProxyBase:BindCommand(command: SimpleCommandType)
    if not command then
        return
    end

    command.ExecuteChanged = {this = self, callback = self.OnExecuteChanged}
end

function UnityEventProxyBase:UnbindCommand(command: SimpleCommandType)
    if not command then
        return
    end

    command.ExecuteChanged = {this = self}
end

function UnityEventProxyBase:dtor()
end

return UnityEventProxyBase