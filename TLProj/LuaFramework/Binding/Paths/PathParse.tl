

require("OOP.Class")

local StringExtension: StringExtensionType

local PathParse = class("PathParse") as PathParseType

function PathParse.Parse(pathText: string): {string}
    return StringExtension.Split(pathText, ".")
end

return PathParse