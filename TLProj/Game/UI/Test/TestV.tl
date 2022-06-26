require("OOP.Class")

local TestVM = require("Game.UI.Test.TestVM")
local WindowBase = require("LuaFramework.Views.WindowBase")
local BindingMode = require("LuaFramework.Binding.BindingMode")
local BindingExtension = require("LuaFramework.Binding.BindingExtension")

local TextExt = require("LuaFramework.Views.Components.TextExt")
local ButtonExt = require("LuaFramework.Views.Components.ButtonExt")
local InputFieldExt = require("LuaFramework.Views.Components.InputFieldExt")

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
    print("hahaha")
end

function TestV:OnCreate()
    WindowBase.OnCreate(self)
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

    self.viewModel.text:BindWithName(self, TextExt, "text", BindingMode.OneWay)
    self.viewModel.button:BindWithName(self, ButtonExt, "button", BindingMode.OneWay)
    -- self.viewModel.inputField:BindWithName(self, InputFieldExt, "inputField", BindingMode.OneWay)

    --
    self.bindingSet:Build()
end

function TestV:OnShow()
    WindowBase.OnShow(self)

    self.viewModel.text.Text = "hahaha"

    -- local transOne = self.transform:Find("GameObjectOne")
    -- local transTwo = self.transform:Find("GameObjectTwo")
    -- self:CreateSubView("Game.TestSubV", {parent = transOne})
    -- self:CreateSubView("Game.TestSubV", {parent = transTwo})
end

function TestV:dtor()
end

return TestV