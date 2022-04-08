using CFM.Framework.Views.Animations;

namespace CFM.Framework.Views
{
    public interface IWindowView
    {
        IAnimation ActivationAnimation{ get; set; }

        IAnimation PassivationAnimation { get; set; }
    }
}

