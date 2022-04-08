using UnityEngine;
using System.Collections.Generic;

namespace CFM.Framework.Views
{
    public delegate void Layout(Transform transform);

    public interface IViewGroup: IView
    {
        List<IView> View { get; }

        IView GetView(string name);

        void AddView(IView view, bool worldPositionStays = false);

        void AddView(IView view, Layout layout);

        void RemoveView(IView view, bool worldPositionStays = false);
    }
}
