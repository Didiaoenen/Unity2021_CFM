using CFM.Framework.Services;
using CFM.Framework.Binding;
using CFM.Framework.Binding.Paths;
using CFM.Framework.Binding.Binders;
using CFM.Framework.Binding.Registry;
using CFM.Framework.Binding.Converters;
using CFM.Framework.Binding.Proxy.Sources;
using CFM.Framework.Binding.Proxy.Sources.Text;
using CFM.Framework.Binding.Proxy.Sources.Object;
using CFM.Framework.Binding.Proxy.Sources.Expressions;
using CFM.Framework.Binding.Proxy.Targets;
using CFM.Framework.Binding.Proxy.Targets.Universal;

namespace CFM.Framework.Binding
{
    public class BindingServiceBundle: AbstractServiceBundle
    {
        public BindingServiceBundle(IServiceContainer container): base(container)
        {

        }

        protected override void OnStart(IServiceContainer container)
        {
            PathParser pathParser = new PathParser();
            ExpressionPathFinder expressionPathFinder = new ExpressionPathFinder();
            ConverterRegistry converterRegistry = new ConverterRegistry();

            ObjectSourceProxyFactory objectSourceProxyFactory = new ObjectSourceProxyFactory();
            objectSourceProxyFactory.Register(new UniversalNodeProxyFactory(), 0);

            SourceProxyFactory sourceFactroy = new SourceProxyFactory();
            sourceFactroy.Register(new LiteralSourceProxyFactory(), 0);
            sourceFactroy.Register(new ExpressionSourceProxyFactory(sourceFactroy, expressionPathFinder), 1);
            sourceFactroy.Register(objectSourceProxyFactory, 2);

            TargetProxyFactory targetFactory = new TargetProxyFactory();
            targetFactory.Register(new UniversalTargetProxyFactory(), 0);
            targetFactory.Register(new UnityTargetProxyFactory(), 10);
#if UNITY_2019_1_OR_NEWER
                targetFactory.Register(new VisualElementProxyFactory(), 30);
#endif

            BindingFactory bindingFactory = new BindingFactory(sourceFactroy, targetFactory);
            StandardBinder binder = new StandardBinder(bindingFactory);

            container.Register<IBinder>(binder);
            container.Register<IBindingFactory>(bindingFactory);
            container.Register<IConverterRegistry>(converterRegistry);
            
            container.Register<IExpressionPathFinder>(expressionPathFinder);
            container.Register<IPathParser>(pathParser);

            container.Register<INodeProxyFactory>(objectSourceProxyFactory);
            container.Register<INodeProxyFactoryRegister>(objectSourceProxyFactory);

            container.Register<ISourceProxyFactory>(sourceFactroy);
            container.Register<ISourceProxyFactoryRegistry>(sourceFactroy);

            container.Register<ITargetProxyFactory>(targetFactory);
            container.Register<ITargetProxyFactoryRegister>(targetFactory);
        }

        protected override void OnStop(IServiceContainer container)
        {
            container.Unregister<IBinder>();
            container.Unregister<IBindingFactory>();
            container.Unregister<IConverterRegistry>();
            
            container.Unregister<IExpressionPathFinder>();
            container.Unregister<IPathParser>();

            container.Unregister<INodeProxyFactory>();
            container.Unregister<INodeProxyFactoryRegister>();

            container.Unregister<ISourceProxyFactory>();
            container.Unregister<ISourceProxyFactoryRegistry>();

            container.Unregister<ITargetProxyFactory>();
            container.Unregister<ITargetProxyFactoryRegister>();
        }
    }
}

