using CFM.Framework.Views.Animations;

namespace CFM.Framework.Views
{
    public interface IWindowView : IUIViewGroup
    {
        IAnimation ActivationAnimation{ get; set; }

        IAnimation PassivationAnimation { get; set; }
    }
}

