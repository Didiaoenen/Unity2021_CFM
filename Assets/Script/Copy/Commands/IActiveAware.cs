using System;

namespace CFM.Framework.Commands
{
    public interface IActiveAware
    {
        bool IsActive { get; set; }

        event EventHandler IsActiveChanged;
    }
}

