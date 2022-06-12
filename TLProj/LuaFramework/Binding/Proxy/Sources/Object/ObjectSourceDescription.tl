

require("OOP.Class")

local SourceDescription = require("LuaFramework.Binding.Proxy.Sources.SourceDescription")

local ObjectSourceDescription = class("ObjectSourceDescriptionType", SourceDescription) as ObjectSourceDescriptionType

function ObjectSourceDescription.get:Path(): {string}
    return self.path
end

function ObjectSourceDescription.set:Path(value: {string})
    self.path = value
end

function ObjectSourceDescription:ctor(path: {string})
    SourceDescription.ctor(self)
    self.path = path
    self.isStatic = false
end

function ObjectSourceDescription:dtor()
end

return ObjectSourceDescription