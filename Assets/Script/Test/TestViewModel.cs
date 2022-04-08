using System.Linq;
using System.Collections.ObjectModel;
using UnityEngine;

class TestViewModel : ABehaviourViewModel, IParentVm
{
    private string _testProperty = @"ÄãºÃ°¡";
    public string TestProperty
    {
        get { return _testProperty; }
        set { SetProperty("TestProperty", ref _testProperty, value); }
    }

    private Color _testColor;
    public Color TestColor
    {
        get { return _testColor; }
        set { SetProperty("TestColor", ref _testColor, value); }
    }

    private ChildViewModel _selected;
    public ChildViewModel Selected
    {
        get { return _selected; }
        set { SetProperty("Selected", ref _selected, value); }
    }

    public Color tColor = Color.white;

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

    private void Update()
    {
        TestColor = tColor;
    }
}
