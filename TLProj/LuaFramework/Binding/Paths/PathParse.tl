

require("OOP.Class")

local PathParse = class("PathParse") as PathParseType

function PathParse.Parse(pathText: string): {string}
    return split(pathText, ".")
end

return PathParse