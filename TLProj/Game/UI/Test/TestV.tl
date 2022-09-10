local TestVM = require("Game.UI.Test.TestVM")
local WindowBase = require("LuaFramework.Views.WindowBase")
local BindingMode = require("LuaFramework.Binding.BindingMode")
local BindingExtension = require("LuaFramework.Binding.BindingExtension")

local TextExt = require("LuaFramework.Views.Components.TextExt")
local ButtonExt = require("LuaFramework.Views.Components.ButtonExt")
local InputFieldExt = require("LuaFramework.Views.Components.InputFieldExt")
local GameObjectExt = require("LuaFramework.Views.Components.GameObjectExt")
local ToggleExt = require("LuaFramework.Views.Components.ToggleExt")

local SimpleCommand = require("LuaFramework.Commands.SimpleCommand")

local TestV = class("TestV", WindowBase) as TestVType

TestV.PrefabPath = "TestV"

function TestV:ctor(bundle)
    WindowBase.ctor(self, bundle)

    self.viewModel = TestVM.new()
    self.bindingExt = BindingExtension.new(self)
    self.bindingSet = self.bindingExt:CreateBindingSetWithContext(self.viewModel as LuaObject) as BindingSetBaseType
    -- local builder = set:Bind()
    -- builder:For("TestFunc"):To("TestRequest")

    -- local builder = set:BindByTarget(text)
    -- builder:For("text"):To("Test"):OneWay()
    
    -- local builder = set:BindByTarget(text)
    -- builder:For("color"):To("TestColor"):OneWay()

    -- local builder = set:BindByTarget(text.transform)
    -- builder:For("localScale"):To("TestLocalScale"):OneWay()

    -- local builder = set:BindByTarget(text.gameObject)
    -- builder:For("activeSelf"):To("TestActive"):TwoWay()

    -- local builder = set:BindByTarget(button)
    -- builder:For("onClick"):To("OnClick"):OneWay()

    -- local builder = set:BindByTarget(button)
    -- builder:For("onClick"):To("TestCommand"):OneWay()

    -- local builder = set:BindByTarget(inputField)
    -- builder:For("text", "onValueChanged"):To("Test"):TwoWay()

    -- set:Build()

    -- testVM.Test = "123"

    -- testVM.TestRequest:Raise()
end

function TestV:TestFunc(sender: any, args: any)
    assert({sender, args}, "")
    print("hahaha")
end

function TestV:OnCreate()
    WindowBase.OnCreate(self as WindowBaseType)
    print("child OnCreate")

    -- local text = self:GetComponentWithName(TextExt, "text") as TextExtType
    -- local button = self:GetComponentWithName(ButtonExt, "button") as ButtonExtType

    -- local bindingSet = self.bindingSet as BindingSetType

    -- local builder = bindingSet:BindByTarget(text as LuaObject)
    -- builder:For(text.Text):To("text.Text"):OneWay()

    -- builder = bindingSet:BindByTarget(text.gameObject as LuaObject)
    -- builder:For(text.GameObject.ActiveSelf):To("text.GameObject.ActiveSelf"):OneWay()
    
    -- builder = bindingSet:BindByTarget(button as LuaObject)
    -- builder:For(button.OnClick):To("TestCommand"):OneWay()

    self.viewModel.text:BindWithName(self as WindowViewBaseType, TextExt as ComponentExtType, "text", {BindingMode.OneWay})
    self.viewModel.button:BindWithName(self as WindowViewBaseType, ButtonExt as ComponentExtType, "button", {BindingMode.OneWay})
    self.viewModel.inputField:BindWithName(self as WindowViewBaseType, InputFieldExt as ComponentExtType, "inputField", {BindingMode.OneWay})
    self.viewModel.gameObject:BindWithName(self as WindowViewBaseType, GameObjectExt, "gameObject", {BindingMode.OneWay})
    self.viewModel.toggle_1:BindWithName(self as WindowViewBaseType, ToggleExt as ComponentExtType, "toggle_1", {BindingMode.OneWay})
    self.viewModel.toggle_2:BindWithName(self as WindowViewBaseType, ToggleExt as ComponentExtType, "toggle_2", {BindingMode.OneWay})

    --
    self.bindingSet:Build()
end

function TestV:OnShow()
    WindowBase.OnShow(self as WindowBaseType)

    self.viewModel.text.Text = "hahaha"
    self.viewModel.button.SimpleCommand = SimpleCommand.new(bindevent(self, self.TestButtonSimpleCommand))
    self.viewModel.inputField.Text = "你好啊"
    self.viewModel.gameObject.ActiveSelf = false
    self.viewModel.toggle_1.SimpleCommand = SimpleCommand.new(bindevent(self, self.TestToggleSimpleCommand, "toggle_1"))
    self.viewModel.toggle_2.SimpleCommand = SimpleCommand.new(bindevent(self, self.TestToggleSimpleCommand, "toggle_2"))

    -- local inputBase = self.viewModel.inputField as BindComponentType
    -- inputBase.GameObject.ActiveSelf = false

    -- local transOne = self.transform:Find("GameObjectOne")
    -- local transTwo = self.transform:Find("GameObjectTwo")
    -- self:CreateSubView("Game.TestSubV", {parent = transOne})
    -- self:CreateSubView("Game.TestSubV", {parent = transTwo})
end

function TestV:TestButtonSimpleCommand(... :any)
    for key, value in ipairs({...}) do
        print("key : " .. key .. " value : " .. tostring(value))
    end
    print("TestButtonSimpleCommand")
end

function TestV:TestToggleSimpleCommand(...: any)
    for key, value in ipairs({...}) do
        print("key : " .. key .. " value : " .. tostring(value))
    end
    print("TestToggleSimpleCommand")
end

function TestV:dtor()
end

return TestV