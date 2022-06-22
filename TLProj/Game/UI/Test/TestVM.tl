require("OOP.Class")

local ViewModelBase = require("LuaFramework.Views.ViewModelBase")
local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")
local InteractionRequest = require("LuaFramework.Interactivity.InteractionRequest")

local TestVM = class("TestVM", ViewModelBase) as TestVMType

function TestVM.get:Test(): string
    return self.test
end

function TestVM.set:Test(value)
    self:Set({name = "test"}, value, "Test")
end

function TestVM.get:TestColor(): CS.UnityEngine.Color
    return self.testColor
end

function TestVM.set:TestColor(value)
    self:Set({name = "testColor"}, value, "TestColor")
end

function TestVM.get:TestLocalScale(): CS.UnityEngine.Vector3
    return self.testLocalScale
end

function TestVM.set:TestLocalScale(value)
    self:Set({name = "testLocalScale"}, value, "TestLocalScale")
end

function TestVM.get:TestActive(): boolean
    return self.testActive
end

function TestVM.set:TestActive(value)
    self:Set({name = "testActive"}, value, "TestActive")
end

function TestVM.get:TestRequest(): InteractionRequestType
    return self.testRequest
end

function TestVM.set:TestRequest(value: InteractionRequestType)
    self.testRequest = value
end

function TestVM.get:TestCommand(): SimpleCommandType
    return self.testCommand
end

function TestVM.set:TestCommand(value: SimpleCommandType)
    self.testCommand = value
end

function TestVM:ctor()
    ViewModelBase.ctor(self)

    --
    self.test = ""
    self.testColor = CS.UnityEngine.Color.red
    self.testLocalScale = CS.UnityEngine.Vector3(0.5, 0.5, 0.5)
    self.testActive = false
    self.testRequest = InteractionRequest.new(self)
    self.testCommand = SimpleCommand.new(function ()
        print("SimpleCommand")
    end)
end

function TestVM:OnClick()
    print("OnClick")
end

function TestVM:dtor()
end

return TestVM