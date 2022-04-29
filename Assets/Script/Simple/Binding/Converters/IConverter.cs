namespace Assembly_CSharp.Assets.Script.Simple.Binding.Converters
{
    public interface IConverter
    {
        object Convert(object value);

        object ConvertBack(object value);
    }

    public interface IConverter<TFrom, TTo> : IConverter
    {
        TTo Convert(TFrom value);

        TFrom ConvertBack(TTo value);
    }
}

