

require("OOP.Class")

local DefaultUIViewLocator = class("DefaultUIViewLocator") as DefaultUIViewLocatorType

function DefaultUIViewLocator:ctor()
    
end

function DefaultUIViewLocator:__singleton(): DefaultUIViewLocatorType
    return DefaultUIViewLocator.new()
end

function DefaultUIViewLocator:LoadAsync(name: string, type: any): ProgressResult_1_Single
    local result = classget(CSharpType.Asynchronous, CSharpTypeName.ProgressResult_1_Single) as ProgressResult_1_Single
    ExecutorsExt:RunOnCoroutineNoReturn(CoroutineExt.GetEnumerator(function ()
        self:DoLoad(result, name, type)
    end))
    return result
end

function DefaultUIViewLocator:DoLoad(promise: ProgressResult_1_Single, name: string, type: any)
    promise:UpdateProgress(0)
    local request = CSharpType.Resources.LoadAsync(name, type) as ResourceRequest
    while not request.isDone do
        promise:UpdateProgress(request.progress)
        CoroutineExt.Wait(3)
    end

    local ins = CSharpType.Object.Instantiate(request.asset as Object)
    ins.name = name

    promise:UpdateProgress(1)
    promise:SetResult(ins)
end

function DefaultUIViewLocator:dtor()
end

return DefaultUIViewLocator