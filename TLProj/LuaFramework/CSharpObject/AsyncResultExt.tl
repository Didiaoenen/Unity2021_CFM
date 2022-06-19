

require("OOP.Class")

local AsyncResultExt = class("AsyncResultExt") as AsyncResultExtType

function AsyncResultExt.get:IsDone(): boolean
    return self.csharp.done
end

function AsyncResultExt.get:Result(): Object
    return self.csharp.result
end

function AsyncResultExt.get:IsCancellationRequested(): boolean
    return self.csharp.cancellationRequested
end

function AsyncResultExt.get:IsCancelled(): boolean
    return self.csharp.cancelled
end

function AsyncResultExt:ctor(cancelable: boolean)
    self.csharp = CSharpType.Asynchronous.AsyncResult(cancelable or false)
end

function AsyncResultExt:SetException(errorMsg: string)
    error(errorMsg)
end

function AsyncResultExt:SetResult(result: Object)
    self.csharp:SetResult(result)
end

function AsyncResultExt:SetCancelled()
    self.csharp:SetCancelled()
end

function AsyncResultExt:Cancel(): boolean
    return self.csharp:Cancel()
end

function AsyncResultExt:Callbackable(): Object
    return self.csharp:Callbackable()
end

function AsyncResultExt:Synchronized(): Object
    return self.csharp:Synchronized()
end

function AsyncResultExt:WaitForDone(): Object
    return self.csharp:WaitForDone()
end

function AsyncResultExt:dtor()
end

return AsyncResultExt