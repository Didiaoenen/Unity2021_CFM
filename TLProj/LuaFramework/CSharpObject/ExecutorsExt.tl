

require("OOP.Class")

local ExecutorsExt = class("ExecutorsExtExt") as ExecutorsExtType

function ExecutorsExt:ctor()
    self.csharp = CSharpType.Execution.Executors
end

function ExecutorsExt:__singleton(): ExecutorsExtType
    return ExecutorsExt.new()
end

function ExecutorsExt:WaitWhile(callback: function): ObjectType
    return self.csharp.WaitWhile(callback)
end

function ExecutorsExt:RunOnMainThread(callback: function)
    self.csharp.RunOnMainThread(callback)
end

function ExecutorsExt:RunOnCoroutine(callback: function): ObjectType
    return self.csharp.RunOnCoroutine(callback)
end

function ExecutorsExt:RunOnCoroutineNoReturn(callback: function)
    self.csharp.RunOnCoroutineNoReturn(callback)
end

function ExecutorsExt:dtor()
end

return ExecutorsExt