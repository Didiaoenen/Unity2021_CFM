local Object = require("OOP.ObjectType")
local Executors = require("OOP.ExecutorsType")

local record ExecutorsExt
    
    csharp: Executors

    __singleton: function(ExecutorsExt): ExecutorsExt
    
    new: function(...: any): ExecutorsExt
    ctor: function
    dtor: function
    WaitWhile: function(ExecutorsExt, function): Object
    RunOnMainThread: function(ExecutorsExt, function)
    RunOnCoroutine: function(ExecutorsExt, function): Object
    RunOnCoroutineNoReturn: function(ExecutorsExt, function)
end

return ExecutorsExt