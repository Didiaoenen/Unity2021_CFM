global Type = require("OOP.Type")

local record Object
    is: function(any): boolean
    GetType: function(Object): Type
end

return Object