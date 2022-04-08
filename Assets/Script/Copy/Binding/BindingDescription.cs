using CFM.Framework.Binding.Proxy.Sources;

namespace CFM.Framework.Binding
{
    public class BindingDescription
    {
        public string TargetName { get; set; }

        public string UpdateTrigger { get; set; }

        public BindingMode Mode { get; set; }

        public SourceDescription Souce { get; set; }

        public object CommandParameter { get; set; }

        public BindingDescription()
        {

        }


    }
}

