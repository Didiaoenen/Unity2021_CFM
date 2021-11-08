using System.Linq;
using System.Collections.ObjectModel;

class TestViewModel : ABehaviourViewModel, IParentVm
{
    private string _testProperty;
    public string TestProperty
    {
        get { return _testProperty; }
        set { SetProperty("TestProperty", ref _testProperty, value); }
    }

    private ChildViewModel _selected;
    public ChildViewModel Selected
    {
        get { return _selected; }
        set { SetProperty("Selected", ref _selected, value); }
    }

    private ObservableCollection<ChildViewModel> _children;
    public ObservableCollection<ChildViewModel> Children
    {
        get { return _children; }
        set { SetProperty("Children", ref _children, value); }
    }

    public TestViewModel()
    {
        Children = new ObservableCollection<ChildViewModel>();
    }

    public void RemoveChild()
    {
        if (Children.Count == 0) return;
        Children.RemoveAt(Children.Count - 1);
    }

    public void RemoveChild(ChildViewModel child)
    {
        Children.Remove(child);
    }

    public void TestReset()
    {
        var childs = Children.ToList();

        childs.RemoveAt(0);
        childs.Add(new ChildViewModel(this));

        Children = new ObservableCollection<ChildViewModel>(childs);
    }
}
