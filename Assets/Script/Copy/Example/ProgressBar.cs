using CFM.Framework.ViewModels;

namespace CFM.Framework.Example
{
    public class ProgressBar: ViewModelBase
    {
        private float progress;

        private string tip;

        private bool enable;

        public bool Enable
        {
            get { return enable; }
            set { Set(ref enable, value, "Enable"); }
        }

        public float Progress
        {
            get { return progress; }
            set { Set(ref progress, value, "Progress"); }
        }

        public string Tip
        {
            get { return tip; }
            set { Set(ref tip, value, "Tip"); }
        }
    }
}

