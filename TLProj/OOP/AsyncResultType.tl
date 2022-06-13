local Object = require("OOP.ObjectType")

local record AsyncResult
    done: boolean
    result: Object
    cancellationRequested: boolean
    cancelled: boolean
    SetResult: function(AsyncResult, Object)
    SetCancelled: function(AsyncResult)
    Cancel: function(AsyncResult): boolean
    Callbackable: function(AsyncResult): Object
    Synchronized: function(AsyncResult): Object
    WaitForDone: function(AsyncResult): Object

    metamethod __call: function(AsyncResult, ...: any): AsyncResult
end

return AsyncResult