using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ABehaviourViewModel : MonoBehaviour, INotifyPropertyChanged
{
    protected void SetProperty<T>(string propertyName, ref T backing, T value)
    {
        if (EqualityComparer<T>.Default.Equals(backing, value)) return;
        backing = value;
        RaisePropertyChanged(propertyName);
    }

    protected void RaisePropertyChanged(string path)
    {
        if (PropertyChanged == null) return;
        PropertyChanged(this, new PropertyChangedEventArgs(path));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
