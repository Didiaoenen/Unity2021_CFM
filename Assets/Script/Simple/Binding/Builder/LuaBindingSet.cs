using Assembly_CSharp.Assets.Script.Simple.Binding.Contexts;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Builder
{
    public class LuaBindingSet : BindingSetBase
    {
        private object target;

        public LuaBindingSet(IBindingContext context, object target) : base(context)
        {
            this.target = target;
        }

        public virtual LuaBindingBuilder Bind()
        {
            var builder = new LuaBindingBuilder(context, target);
            builders.Add(builder);
            return builder;
        }

        public virtual LuaBindingBuilder Bind(object target)
        {
            var builder = new LuaBindingBuilder(context, target);
            builders.Add(builder);
            return builder;
        }
    }
}

