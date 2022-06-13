local Object = require("OOP.ObjectType")
local AsyncResult = require("OOP.AsyncResultType")

local record Get
    csharp: AsyncResult

    IsDone: function
    Result: function
    IsCancellationRequested: function
    IsCancelled: function
end

local record AsyncResultExt
    get: Get

    csharp: AsyncResult

    ctor: function
    dtor: function
    SetException: function(AsyncResultExt, string)
    SetResult: function(AsyncResultExt, Object)
    SetCancelled: function(AsyncResultExt)
    Cancel: function(AsyncResult): boolean
    Callbackable: function(AsyncResult): Object
    Synchronized: function(AsyncResult): Object
    WaitForDone: function(AsyncResult): Object
end

return AsyncResultExt