local record TestV
    PrefabPath: string

    viewModel: TestVMType
    bindingExt: BindingExtensionType
    bindingSet: BindingSetBaseType

    new: function(...: any): TestV
    ctor: function
    dtor: function

    Create: function(self: TestV)
    OnCreate: function(self: TestV)
    OnShow: function(self: TestV)
    GetComponentWithName: function(self: TestV, ComponentExtType, string): ComponentExtType
    TestFunc: function(self: TestV, any, any)
end

return TestV