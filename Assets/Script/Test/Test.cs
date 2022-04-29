using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly_CSharp.Assets.Script.Simple.Binding;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Binding.Binders;
using Assembly_CSharp.Assets.Script.Simple.Binding.Builder;
using Assembly_CSharp.Assets.Script.Simple.Binding.Converters;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UGUI;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UIElements;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Text;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions;

public class TestVM
{
    private InteractionRequest testRequest;

    public IInteractionRequest TestRequest
    {
        get { return testRequest; }
    }

    public TestVM()
    {
        testRequest = new InteractionRequest(this);
    }
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void TestClick()
    {
        PathParser pathParser = new PathParser();
        ExpressionPathFinder expressionPathFinder = new ExpressionPathFinder();
        ConverterRegistry converterRegistry = new ConverterRegistry();

        UniversalNodeProxyFactory universalNodeProxyFactory = new UniversalNodeProxyFactory();
        ObjectSourceProxyFactory objectSourceProxyFactory = new ObjectSourceProxyFactory();
        objectSourceProxyFactory.Register(universalNodeProxyFactory, 0);

        SourceProxyFactory sourceProxyFactory = new SourceProxyFactory();
        sourceProxyFactory.Register(new LiteralSourceProxyFactory(), 0);
        sourceProxyFactory.Register(new ExpressionSourceProxyFactory(sourceProxyFactory, expressionPathFinder), 1);
        sourceProxyFactory.Register(objectSourceProxyFactory, 1);

        TargetProxyFactory targetProxyFactory = new TargetProxyFactory();
        targetProxyFactory.Register(new UniversalTargetProxyFactory(), 0);
        targetProxyFactory.Register(new UnityTargetProxyFactory(), 10);
        targetProxyFactory.Register(new VisualElementProxyFactory(), 30);

        BindingFactory bindingFactory = new BindingFactory(sourceProxyFactory, targetProxyFactory);
        StandardBinder binder = new StandardBinder(bindingFactory);

        BehaviourBindingExtension.Binder = binder;

        TestVM testVM = new TestVM();
        BindingSet<Test, TestVM> bindingSet = this.CreateBindingSet(testVM);
        var builder = bindingSet.Bind();
        builder.PathParser = pathParser;
        builder.ConverterRegistry = converterRegistry;
        builder.For(v => v.TestFunc).To(vm => vm.TestRequest);
        bindingSet.Build();

        ((InteractionRequest)testVM.TestRequest).Raise();
    }

    protected void TestFunc(object sender, InteractionEventArgs args)
    {
        Debug.Log(args);
    }
}

