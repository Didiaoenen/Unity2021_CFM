

require("OOP.Class")

local PathParse: PathParseType

local BindingDescription = require("LuaFramework.Binding.BindingDescription")
local ObjectSourceDescription = require("LuaFramework.Binding.Proxy.Sources.Object.ObjectSourceDescription")

local BindingBuilderBase = class("BindingBuilderBase") as BindingBuilderBaseType

function BindingBuilderBase:ctor(context: BindingContextType, target: BaseType)
    assert(context and target, "")

    self.context = context
    self.target = target

    self.builded = false
    self.scopeKey = ""
    self.description = BindingDescription.new()
    self.description.Mode = BindingMode.Default
end

function BindingBuilderBase:SetMode(mode: number)
    self.description.Mode = mode
end

function BindingBuilderBase:SetMemberPath(pathText: string)
    local path = PathParse.Parse(pathText)
    self:SetMemberPathWithPath(path)
end

function BindingBuilderBase:SetMemberPathWithPath(path: {string})
    self.description.Source = ObjectSourceDescription.new(path) as SourceDescriptionType
end

function BindingBuilderBase:SetCommandParameter(...: any)
    print(...)
end

function BindingBuilderBase:CheckBindingDescription()
    assert(self.description.TargetName and self.description.TargetName ~= "", "")
    assert(self.description.Source, "")
end

function BindingBuilderBase:Build()
    if self.builded then
        return                
    end

    self:CheckBindingDescription()
    self.context:AddWithSingleDesc(self.target, self.description, self.scopeKey)
    self.builded = true
end

function BindingBuilderBase:dtor()
end

return BindingBuilderBase