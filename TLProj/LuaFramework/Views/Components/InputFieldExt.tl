local ComponentExt = require("LuaFramework.Views.Components.ComponentExt")

local InputFieldExt = class("InputFieldExt", ComponentExt) as InputFieldExtType

InputFieldExt.CSObject = CS.UnityEngine.UI.InputField as CS.UnityEngine.Component

InputFieldExt.Text = "text"

InputFieldExt.OnValueChanged = "onValueChanged"

function InputFieldExt.get:text(): string
    return (self.handler as CS.UnityEngine.UI.InputField).text
end

function InputFieldExt.set:text(value: string)
    (self.handler as CS.UnityEngine.UI.InputField).text = value
end

function InputFieldExt.get:onValueChanged(): CS.UnityEngine.UI.InputField.OnChangeEvent
    return (self.handler as CS.UnityEngine.UI.InputField).onValueChanged
end

function InputFieldExt.set:onValueChanged(value: CS.UnityEngine.UI.InputField.OnChangeEvent)
    (self.handler as CS.UnityEngine.UI.InputField).onValueChanged = value
end

function InputFieldExt.get:GameObject(): GameObjectExtType
    return self.gameObject
end

function InputFieldExt:ctor(handler: CS.UnityEngine.Component)
    ComponentExt.ctor(self, handler)
end

return InputFieldExt