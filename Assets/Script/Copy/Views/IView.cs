using UnityEngine;

namespace CFM.Framework.Views
{
    public interface IView
    {
        string Name { get; set; }

        Transform Parent { get; }

        GameObject Owner { get; }

        Transform Transform { get; }

        bool Visibility { get; set; }

        IAttributes ExtraAttributes { get; }
    }
}