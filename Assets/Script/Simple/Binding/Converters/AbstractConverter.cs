namespace Assembly_CSharp.Assets.Script.Simple.Binding.Converters
{
    public abstract class AbstractConverter : IConverter
    {
        public virtual object Convert(object value)
        {
            return value;
        }

        public virtual object ConvertBack(object value)
        {
            return value;
        }
    }

    public abstract class AbstractConverter<TFrom, TTo> : IConverter<TFrom, TTo>
    {
        public virtual TTo Convert(TFrom value)
        {
            throw new System.NotImplementedException();
        }

        public virtual object Convert(object value)
        {
            return Convert((TFrom)value);
        }

        public virtual TFrom ConvertBack(TTo value)
        {
            throw new System.NotImplementedException();
        }

        public virtual object ConvertBack(object value)
        {
            return ConvertBack((TTo)value);
        }
    }
}

