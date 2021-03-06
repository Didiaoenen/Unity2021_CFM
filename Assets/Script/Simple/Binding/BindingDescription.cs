using System.Text;
using Assembly_CSharp.Assets.Script.Simple.Binding.Paths;
using Assembly_CSharp.Assets.Script.Simple.Binding.Converters;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources;
using Assembly_CSharp.Assets.Script.Simple.Binding.Proxy.Sources.Object;

namespace Assembly_CSharp.Assets.Script.Simple.Binding
{
    public class BindingDescription
    {
        public string TargetName { get; set; }

        public string UpdateTrigger { get; set; }

        public IConverter Converter { get; set; }

        public BindingMode Mode { get; set; }

        public SourceDescription Source { get; set; }

        public object CommandParameter { get; set; }

        public BindingDescription()
        {

        }

        public BindingDescription(string targetName, Path path, IConverter converter = null, BindingMode mode = BindingMode.Default)
        {
            TargetName = targetName;
            Mode = mode;
            Converter = converter;
            Source = new ObjectSourceDescription
            {
                Path = path
            };
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("{binding ").Append(TargetName);

            if (!string.IsNullOrEmpty(UpdateTrigger))
                buf.Append(" UpdateTrigger:").Append(UpdateTrigger);

            if (Converter != null)
                buf.Append(" Converter:").Append(Converter.GetType().Name);

            if (Source != null)
                buf.Append(" ").Append(Source.ToString());

            if (CommandParameter != null)
                buf.Append(" CommandParameter:").Append(CommandParameter);

            buf.Append(" Mode:").Append(Mode.ToString());
            buf.Append(" }");
            return buf.ToString();
        }
    }
}

