namespace Assembly_CSharp.Assets.Script.Simple.Binding.Proxy
{
    public interface IObtainable
    {
        object GetValue();

        TValue GetValue<TValue>();
    }
}

