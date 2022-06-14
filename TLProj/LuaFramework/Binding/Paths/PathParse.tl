

require("OOP.Class")

local PathParse = class("PathParse") as PathParseType

function PathParse.Parse(pathText: string): {string}
    return StringExtension.Split(pathText, ".")
end

return PathParse