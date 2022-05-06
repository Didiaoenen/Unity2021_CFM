using System;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class ProxyEntry : IDisposable
    {
        private ISourceProxy proxy;

        private EventHandler handler;
        
        public ProxyEntry(ISourceProxy proxy, PathToken token)
        {
            Proxy = proxy;
            Token = token;
        }

        public ISourceProxy Proxy
        {
            get { return proxy; }
            set
            {
                if (proxy == value)
                    return;

                if (handler != null)
                {
                    var notifiable = proxy as INotifiable;
                    if (notifiable != null)
                        notifiable.ValueChanged -= handler;

                    notifiable = value as INotifiable;
                    if (notifiable != null)
                        notifiable.ValueChanged += handler;
                }

                proxy = value;
            }
        }

        public PathToken Token { get; set; }

        public EventHandler Handler
        {
            get { return handler; }
            set
            {
                if (handler == value)
                    return;

                var notifiable = proxy as INotifiable;
                if (notifiable != null)
                {
                    if (handler != null)
                        notifiable.ValueChanged -= handler;

                    if (value != null)
                        notifiable.ValueChanged += value;
                }

                handler = value;
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Handler = null;
                if (proxy != null)
                    proxy.Dispose();
                proxy = null;
                Token = null;
                disposedValue = true;
            }
        }

        ~ProxyEntry()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class ChainedObjectSourceProxy : NotifiableSourceProxyBase, IObtainable, IModifiable, INotifiable
    {
        private INodeProxyFactory factory;

        private ProxyEntry[] proxies;

        private int count;

        public override Type Type
        {
            get
            {
                var proxy = GetProxy();
                if (proxy == null)
                    return typeof(object);

                return proxy.Type;
            }
        }

        public override TypeCode TypeCode
        {
            get
            {
                var proxy = GetProxy();
                if (proxy == null)
                    return TypeCode.Object;

                return proxy.TypeCode;
            }
        }

        public ChainedObjectSourceProxy(object source, PathToken token, INodeProxyFactory factory) : base(source)
        {
            this.factory = factory;
            count = token.Path.Count;
            proxies = new ProxyEntry[count];
            Bind(source, token);
        }

        public object GetValue()
        {
            IObtainable obtainable = GetObtainable();
            if (obtainable == null)
                return null;
            return obtainable.GetValue();
        }

        public TValue GetValue<TValue>()
        {
            IObtainable obtainable = GetObtainable();
            if (obtainable == null)
                return default(TValue);

            return obtainable.GetValue<TValue>();
        }

        public void SetValue(object value)
        {
            IModifiable modifiable = GetModifiable();
            if (modifiable == null)
                return;

            modifiable.SetValue(value);
        }

        public void SetValue<TValue>(TValue value)
        {
            IModifiable modifiable = GetModifiable();
            if (modifiable == null)
                return;

            modifiable.SetValue(value);
        }

        protected ISourceProxy GetProxy()
        {
            ProxyEntry proxyEntry = proxies[count - 1];
            if (proxyEntry == null)
                return null;

            return proxyEntry.Proxy;
        }

        protected IObtainable GetObtainable()
        {
            ProxyEntry proxyEntry = proxies[count - 1];
            if (proxyEntry == null)
                return null;

            return proxyEntry.Proxy as IObtainable;
        }

        protected IModifiable GetModifiable()
        {
            ProxyEntry proxyEntry = proxies[count - 1];
            if (proxyEntry == null)
                return null;

            return proxyEntry.Proxy as IModifiable;
        }

        void Bind(object source, PathToken token)
        {
            int index = token.Index;
            ISourceProxy proxy = factory.Create(source, token);
            if (proxy == null)
            {
                var node = token.Current;
                if (node is MemberNode)
                {
                    var memberNode = node as MemberNode;
                    string typeName = source != null ? source.GetType().Name : memberNode.Type.Name;
                    throw new Exception();
                }
                throw new Exception();
            }

            ProxyEntry entry = new ProxyEntry(proxy, token);
            proxies[index] = entry;

            if (token.HasNext())
            {
                if (proxy is INotifiable)
                {
                    entry.Handler = (sender, args) =>
                    {
                        lock (_lock)
                        {
                            try
                            {
                                var proxyEntry = proxies[index];
                                if (proxyEntry == null || sender != proxyEntry.Proxy)
                                    return;

                                Rebind(index);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    };
                }

                var child = (proxy as IObtainable).GetValue();
                if (child != null)
                    Bind(child, token.NextToken());
                else
                    RaiseValueChanged();
            }
            else
            {
                if (proxy is INotifiable)
                    entry.Handler = (sender, args) => { RaiseValueChanged(); };
                RaiseValueChanged();
            }
        }

        void Rebind(int index)
        {
            for (int i = proxies.Length - 1; i > index; i--)
            {
                ProxyEntry proxyEntry = proxies[i];
                if (proxyEntry == null)
                    continue;

                var proxy = proxyEntry.Proxy;
                proxyEntry.Proxy = null;
                if (proxy != null)
                    proxy.Dispose();
            }

            ProxyEntry entry = proxies[index];
            var obtainable = entry.Proxy as IObtainable;
            if (obtainable == null)
            {
                RaiseValueChanged();
                return;
            }

            var source = obtainable.GetValue();
            if (source == null)
            {
                RaiseValueChanged();
                return;
            }

            Bind(source, entry.Token.NextToken());
        }

        void Unbind()
        {
            for (int i = proxies.Length - 1; i >= 0; i--)
            {
                ProxyEntry proxyEntry = proxies[i];
                if (proxyEntry == null)
                    continue;

                proxyEntry.Dispose();
                proxies[i] = null;
            }
        }

        private bool disposedValue = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Unbind();
                disposedValue = true;
                base.Dispose(disposing);
            }
        }
    }
}

