using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class AViewModel : INotifyPropertyChanged
{
    protected bool SetProperty<T>(string propertyName, ref T backing, T value)
    {
        if (EqualityComparer<T>.Default.Equals(backing, value)) return false;
        backing = value;
        RaisePropertyChanged(propertyName);
        return true;
    }

    protected void RaisePropertyChanged(string path)
    {
        if (PropertyChanged == null) return;
        PropertyChanged(this, new PropertyChangedEventArgs(path));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
