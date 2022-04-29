using System;
using System.Collections.Generic;

namespace Assembly_CSharp.Assets.Script.Simple.Binding.Contexts
{
    public interface IBindingContext : IDisposable
    {
        event EventHandler DataContextChanged;

        object Owner { get; }

        object DataContext { get; set; }

        void Add(IBinding binding, object key = null);

        void Add(IEnumerable<IBinding> bindings, object key = null);

        void Add(object target, BindingDescription description, object key = null);

        void Add(object target, IEnumerable<BindingDescription> descritions, object key = null);

        void Clear(object key);

        void Clear();
    }
}

