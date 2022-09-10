local record TestVM
    record get
        test: string
        Test: function

        testColor: CS.UnityEngine.Color
        TestColor: function

        testLocalScale: CS.UnityEngine.Vector3
        TestLocalScale: function

        testActive: boolean
        TestActive: function

        testRequest: InteractionRequestType
        TestRequest: function

        testCommand: SimpleCommandType
        TestCommand: function

        Set: function(get, SetType, any, string, boolean)
    end

    record set
        test: string
        Test: function

        testColor: CS.UnityEngine.Color
        TestColor: function

        testLocalScale: CS.UnityEngine.Vector3
        TestLocalScale: function

        testActive: boolean
        TestActive: function

        testRequest: InteractionRequestType
        TestRequest: function

        testCommand: SimpleCommandType
        TestCommand: function

        Set: function(set, SetType, any, string, boolean)
    end

    test: string
    Test: string

    testColor: CS.UnityEngine.Color
    TestColor: CS.UnityEngine.Color

    testLocalScale: CS.UnityEngine.Vector3
    TestLocalScale: CS.UnityEngine.Vector3

    testActive: boolean
    TestActive: boolean

    testRequest: InteractionRequestType
    TestRequest: InteractionRequestType

    testCommand: SimpleCommandType
    TestCommand: SimpleCommandType

    text: BindTextType
    button: BindButtonType
    inputField: BindInputFieldType
    gameObject: BindGameObjectType
    toggle_1: BindToggleType
    toggle_2: BindToggleType

    new: function(...: any): TestVM
    ctor: function
    dtor: function
    
    OnClick: function(self: TestVM)
end

return TestVM