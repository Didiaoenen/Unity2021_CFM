local Object = require("OOP.ObjectType")

local record Executors
    WaitWhile: function(function): Object
    RunOnMainThread: function(function)
    RunOnCoroutine: function(function): Object
    RunOnCoroutineNoReturn: function(function)
end

return Executors