namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy
{
    public interface IModifiable
    {
        void SetValue(object value);

        void SetValue<TValue>(TValue value);
    }
}

