

require("OOP.Class")

local CoroutineExt = require("LuaFramework.CSharpObject.CoroutineExt")
local ExecutorsExt = require("LuaFramework.CSharpObject.ExecutorsExt")

local DefaultUIViewLocator = class("DefaultUIViewLocator") as DefaultUIViewLocatorType

function DefaultUIViewLocator.LoadAsync(name: string, type: Type): ProgressResult_1_Single
    local result = (classget(CS.CSObject.Asynchronous, CS.CSName.ProgressResult_1_Single) as ProgressResult_1_Single)(true)
    ExecutorsExt.Instance:RunOnCoroutineNoReturn(CoroutineExt.Instance.GetEnumerator(function ()
        DefaultUIViewLocator.DoLoad(result, name, type)
    end))
    return result
end

function DefaultUIViewLocator.DoLoad(promise: ProgressResult_1_Single, name: string, type: Type)
    promise:UpdateProgress(0)
    local request = CS.CSObject.Resources.LoadAsync(name, type) as ResourceRequest
    while not request.isDone do
        promise:UpdateProgress(request.progress)
        CoroutineExt.Instance.Wait(3)
    end

    local ins = CS.CSObject.Object.Instantiate(request.asset as Object)
    ins.name = name

    promise:UpdateProgress(1)
    promise:SetResult(ins)
end

return DefaultUIViewLocator