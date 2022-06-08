using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assembly_CSharp.Assets.Script.Simple.Binding;
using Assembly_CSharp.Assets.Script.Simple.Binding.Binders;
using Assembly_CSharp.Assets.Script.Simple.Binding.Builder;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UGUI;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.Universal;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Targets.UIElements;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Text;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Expressions;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;
using Assembly_CSharp.Assets.Script.Simple.Commands;

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
    private bool testBool;

    private string testString;

    private float testExpression = 10f;

    private SimpleCommand command;

    private InteractionRequest testRequest;

    private Vector3 testScale = Vector3.one;

    public Vector3 TestScale
    {
        get { return testScale; }
        set { Set(ref testScale, value, "TestScale"); }
    }

    public bool TestBool
    {
        get { return testBool; }
        set { Set(ref testBool, value, "TestBool"); }
    }

    public string TestString
    {
        get { return testString; }
        set { Set(ref testString, value, "TestString"); }
    }

    public float TestExpression
    {
        get { return testExpression; }
        set { Set(ref testExpression, value, "TestExpression"); }
    }

    public IInteractionRequest TestRequest
    {
        get { return testRequest; }
    }

    public ICommand TestCommand
    {
        get { return command; }
    }

    public Color TestColor
    {
        get { return Color.red; }
    }

    public TestVM()
    {
        command = new SimpleCommand(() =>
        {
            Debug.Log("TestCommand");
        });

        testRequest = new InteractionRequest(this);
    }

    public void OnClick()
    {
        Debug.Log("OnClick");
    }
}

public class Test : MonoBehaviour
{

    public Text testText;

    public Text testExpression;

    public Button initButton;

    public Button testButton;

    public Button testCommand;

    public InputField testInputField;

    public Image testImage;

    private TestVM testVM;

    // Start is called before the first frame update
    void Start()
    {

        testImage.gameObject.AddComponent(typeof(Canvas));
    }

    public void InitClick()
    {
        UniversalNodeProxyFactory universalNodeProxyFactory = new UniversalNodeProxyFactory();
        ObjectSourceProxyFactory objectSourceProxyFactory = new ObjectSourceProxyFactory();
        objectSourceProxyFactory.Register(universalNodeProxyFactory, 0);

        SourceProxyFactory sourceProxyFactory = new SourceProxyFactory();
        sourceProxyFactory.Register(new LiteralSourceProxyFactory(), 0);
        sourceProxyFactory.Register(new ExpressionSourceProxyFactory(sourceProxyFactory), 1);
        sourceProxyFactory.Register(objectSourceProxyFactory, 2);

        TargetProxyFactory targetProxyFactory = new TargetProxyFactory();
        targetProxyFactory.Register(new UniversalTargetProxyFactory(), 0);
        targetProxyFactory.Register(new UnityTargetProxyFactory(), 10);
        targetProxyFactory.Register(new VisualElementProxyFactory(), 20);

        StandardBinder binder = new StandardBinder(sourceProxyFactory, targetProxyFactory);
        BehaviourBindingExtension.Binder = binder;

        testVM = new TestVM();
        BindingSet<Test, TestVM> bindingSet = this.CreateBindingSet(testVM);
        //var builder = bindingSet.Bind();
        //builder.For(v => v.TestFunc).To(vm => vm.TestRequest);

        //var testBuilder = bindingSet.Bind(testText);
        //testBuilder.For(v => v.text).To(vm => vm.TestString).OneWay();

        //var testButtonBuilder = bindingSet.Bind(testButton);
        //testButtonBuilder.For(v => v.onClick).To(vm => vm.OnClick).OneWay();

        //var testCommandBuilder = bindingSet.Bind(testCommand);
        //testCommandBuilder.For(v => v.onClick).To(vm => vm.TestCommand).OneWay();

        //var testExpressionBuilder = bindingSet.Bind(testExpression);
        //testExpressionBuilder.For(v => v.text).ToExpression(vm => string.Format("{0}%", Mathf.FloorToInt(vm.TestExpression * 100f)));

        //testExpressionBuilder = bindingSet.Bind(testExpression);
        //testExpressionBuilder.For(v => v.color).To(vm => vm.TestColor).OneWay();

        //var testInputFieldBuilder = bindingSet.Bind(testInputField);
        //testInputFieldBuilder.For(v => v.text, v => v.onValueChanged).To(vm => vm.TestString).TwoWay();

        var testGameObjectBuilder = bindingSet.Bind(testInputField.gameObject);
        testGameObjectBuilder.For(v => v.activeSelf).To(vm => vm.TestBool).TwoWay();

        //var testTransformBuilder = bindingSet.Bind(testInputField.transform);
        //testTransformBuilder.For(v => v.localScale).To(vm => vm.TestScale).TwoWay();

        //
        bindingSet.Build();
    }

    public void TestClick()
    {
        ((InteractionRequest)testVM.TestRequest).Raise();
    }

    protected void TestFunc(object sender, InteractionEventArgs args)
    {
        Debug.Log(args);

        testVM.TestString = "hahaha";

        testVM.TestBool = true;

        testVM.TestScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}

