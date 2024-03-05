
local record testClass
    record get
        f: string
        F: function(self:testClass)
    end

    record set
        f: string
        F: function(self:testClass, any)
    end

    F: string

    new: function(testClass, ...: any): testClass
end

local test = class("testClass") as testClass

function test.get:F(): any
    return self.f
end

function test.set:F(value: any)
    self.f = value as string
end

local record testClass1

    F: string

    new: function(testClass1, ...: any): testClass1
end

local test1 = class("testClass1", testClass) as testClass1

return test1