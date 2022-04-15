using UnityEngine;
using System.Collections.Generic;

namespace CFM.Framework.Views
{
    public delegate void UILayout(RectTransform transform);

    public interface IUIViewGroup: IUIView
    {
        List<IUIView> Views{ get; }

        IUIView GetView(string name);

        void AddView(IUIView view, bool worldPositionStays = false);

        void AddView(IUIView view, UILayout layout);

        void RemoveView(IUIView view, bool worldPositionStays = false);
    }
}

