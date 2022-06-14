

require("OOP.Class")

local ComponentExt = class("ComponentExt") as ComponentExtType

function ComponentExt:ctor(handler: Component)
    self.handler = handler
end

function ComponentExt:dtor()
    self.handler = nil
end

return ComponentExt