require("OOP.Class")

local EventTargetProxyBase = require("LuaFramework.Binding.Proxy.Targets.Universal.EventTargetProxyBase")
local ProxyInvoker = require("LuaFramework.Binding.Reflection.ProxyInvoker")
local ExecutorsExt = require("LuaFramework.CSharpObject.ExecutorsExt")
local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")
local BindingMode = require("LuaFramework.Binding.BindingMode")

local UnityEventProxyBase = class("UnityEventProxyBase", EventTargetProxyBase) as UnityEventProxyBaseType

function UnityEventProxyBase.get:DefaultMode(): number
    return BindingMode.OneWay
end

function UnityEventProxyBase:ctor(target: LuaObject, unityEvent: string)
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

function UnityEventProxyBase:IsValidInvoker(invoker: IProxyInvokerType)
    assert({invoker}, "")
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
            if (value as LuaObject).is(ProxyInvoker) then
                self.invoker = value as IProxyInvokerType
            elseif (value as LuaObject).is(SimpleCommand) then
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

function UnityEventProxyBase:OnExecuteChanged(sender: LuaObject, ...: any)
    assert({sender, ...}, "")
    ExecutorsExt.Instance:RunOnMainThread(function ()
        self:UpdateTargetInteractable()
    end)
end

function UnityEventProxyBase:UpdateTargetInteractable()
    local target = self.target
    if nil == self.interactable or self.interactable == "" or nil == target then
        return
    end

    local value = nil == self.command and false or self.command:CanExecute()
    classset(target, self.interactable, value)
end

function UnityEventProxyBase:BindCommand(command: SimpleCommandType)
    if nil == command then
        return
    end

    command.ExecuteChanged = {this = self, callback = self.OnExecuteChanged}
end

function UnityEventProxyBase:UnbindCommand(command: SimpleCommandType)
    if nil == command then
        return
    end

    command.ExecuteChanged = {this = self}
end

function UnityEventProxyBase:dtor()
end

return UnityEventProxyBase