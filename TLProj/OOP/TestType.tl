local record Public
    data: string
    state: number

    PrintMe: function(TestType)
end

local record TestType
    data: string
    state: number
    public: Public

    new: function(TestType): TestType
    PrintMe: function(TestType)
end

return TestType