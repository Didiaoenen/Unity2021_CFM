using XLua;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Interactivity;
using Assembly_CSharp.Assets.Script.Simple.Observables;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object
{
    public class LuaNodeProxyFactory : INodeProxyFactory
    {
        public ISourceProxy Create(object source, PathToken token)
        {
            if (source == null || !(source is LuaTable) || token.Path.IsStatic)
                return null;

            IPathNode node = token.Current;
            LuaTable table = source as LuaTable;
            return CreateProxy(table, node);
        }

        protected virtual bool Contains(LuaTable table, IPathNode node)
        {
            var indexedNode = node as IndexedNode;
            if (indexedNode != null)
                return table.ContainsKey(indexedNode.Value);

            var memberNode = node as MemberNode;
            if (memberNode != null)
                return table.ContainsKey(memberNode.Name);

            return false;
        }

        protected virtual ISourceProxy CreateProxy(LuaTable table, IPathNode node)
        {
            var indexedNode = node as IndexedNode;
            if (indexedNode != null)
            {
                if (indexedNode.Value is int)
                    return new LuaIntTableNodeProxy(table, (int)indexedNode.Value);

                if (indexedNode.Value is string)
                    return new LuaStringTableNodeProxy(table, (string)indexedNode.Value);

                return null;
            }

            var memberNode = node as MemberNode;
            if (memberNode != null)
            {
                var obj = table.Get<object>(memberNode.Name);
                if (obj != null)
                {
                    LuaFunction function = obj as LuaFunction;
                    if (function != null)
                        return new LuaMethodNodeProxy(table, function);

                    IObservableProperty observableValue = obj as IObservableProperty;
                    if (observableValue != null)
                        return new ObservableNodeProxy(table, observableValue);

                    IInteractionRequest request = obj as IInteractionRequest;
                    if (request != null)
                        return new InteractionNodeProxy(table, request);
                }

                return new LuaStringTableNodeProxy(table, memberNode.Name);
            }
            return null;
        }
    }
}

