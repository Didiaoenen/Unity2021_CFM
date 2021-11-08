using Mvvm;

class ChildViewModel : AViewModel
{
    private readonly IParentVm _parent;

    string _testProperty;
    public string TestProperty
    {
        get { return _testProperty; }
        set { SetProperty("TestProperty", ref _testProperty, value); }
    }

    bool _isSomething;
    public bool IsSomething
    {
        get { return _isSomething; }
        set { SetProperty("IsSomething", ref _isSomething, value); }
    }

    private bool _canSomething = true;
    public bool CanSomething
    {
        get { return _canSomething; }
        set
        {
            if (SetProperty("CanSomething", ref _canSomething, value))
                ToggleSomething.RaiseCanExecuteChanged();
        }
    }

    public ChildViewModel(IParentVm parent)
    {
        _parent = parent;
        _testProperty = "";
        ToggleSomething = new RelayCommand(InternalToggleSomething, InternalCanToggleSomething);
    }

    public RelayCommand ToggleSomething { get; private set; }

    void InternalToggleSomething()
    {
        IsSomething = !IsSomething;
    }

    bool InternalCanToggleSomething()
    {
        return _canSomething;
    }

    public RelayCommand Delete
    {
        get { return new RelayCommand(() => _parent.RemoveChild(this)); }
    }
}