using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


public class VMBase : INotifyPropertyChanged
{
    private readonly object _lock = new object();

    private PropertyChangedEventHandler propertyChanged;

    public event PropertyChangedEventHandler PropertyChanged
    {
        add { lock (_lock) { propertyChanged += value; } }
        remove { lock (_lock) { propertyChanged -= value; } }
    }

    protected bool Set<T>(ref T field, T newValue, string propertyName, bool broadcast = false)
    {
        if (Equals(field, newValue))
            return false;

        var oldValue = field;
        field = newValue;
        RaisePropertyChanged(propertyName);

        if (broadcast)
            Broadcast(oldValue, newValue, propertyName);
        
        return true;
    }

    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
        RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void RaisePropertyChanged(PropertyChangedEventArgs eventArgs)
    {
        try
        {
            VerifyPropertyName(eventArgs.PropertyName);

            if (propertyChanged != null)
                propertyChanged(this, eventArgs);
        }
        catch (Exception e)
        {
        }
    }

    protected void VerifyPropertyName(string propertyName)
    {
    }

    protected void Broadcast<T>(T oldValue, T newValue, string propertyName)
    {
        try
        {
        }
        catch (Exception e)
        {
        }
    }
}

public class TestVM : VMBase
{
    private string testString;

    private InteractionRequest testRequest;

    public string TestString
    {
        get { return testString; }
        set { Set(ref testString, value, "TestString"); }
    }

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

    public Text testText;

    private TestVM testVM;

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
        sourceProxyFactory.Register(objectSourceProxyFactory, 2);

        TargetProxyFactory targetProxyFactory = new TargetProxyFactory();
        targetProxyFactory.Register(new UniversalTargetProxyFactory(), 0);
        targetProxyFactory.Register(new UnityTargetProxyFactory(), 10);
        targetProxyFactory.Register(new VisualElementProxyFactory(), 30);

        BindingFactory bindingFactory = new BindingFactory(sourceProxyFactory, targetProxyFactory);
        StandardBinder binder = new StandardBinder(bindingFactory);

        BehaviourBindingExtension.Binder = binder;

        testVM = new TestVM();
        BindingSet<Test, TestVM> bindingSet = this.CreateBindingSet(testVM);
        var builder = bindingSet.Bind();
        builder.PathParser = pathParser;
        builder.ConverterRegistry = converterRegistry;
        builder.For(v => v.TestFunc).To(vm => vm.TestRequest);
        var testBuilder = bindingSet.Bind(testText);
        testBuilder.PathParser = pathParser;
        testBuilder.ConverterRegistry = converterRegistry;
        testBuilder.For(v => v.text).To(vm => vm.TestString).OneWay();
        bindingSet.Build();

        ((InteractionRequest)testVM.TestRequest).Raise();
    }

    protected void TestFunc(object sender, InteractionEventArgs args)
    {
        Debug.Log(args);

        testVM.TestString = "¹þ¹þ¹þ";
    }
}

