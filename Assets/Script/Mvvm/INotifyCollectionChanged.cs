#if !(UNITY_WSA || !NET_LEGACY)
using System;
using System.Collections;

public delegate void NotifyCollectionChangedEventHandler(Object sender, NotifyCollectionChangedEventArgs e);

public interface INotifyCollectionChanged
{
    event NotifyCollectionChangedEventHandler CollectionChanged;
}

public class NotifyCollectionChangedEventArgs : EventArgs
{
    public NotifyCollectionChangedAction Action { get; private set; }

    public IList NewItems { get; private set; }

    public int NewStartingIndex { get; private set; }

    public IList OldItems { get; private set; }

    public int OldStartingIndex { get; private set; }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
    {
        if (action != NotifyCollectionChangedAction.Reset)
            throw new ArgumentException("WrongActionForCtor, NotifyCollectionChangedAction.Reset", "action");

        InitializeAdd(action, null, -1);
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
    {
        if ((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)
                && (action != NotifyCollectionChangedAction.Reset))
            throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action");

        if (action == NotifyCollectionChangedAction.Reset)
        {
            if (changedItem != null)
                throw new ArgumentException("ResetActionRequiresNullItem", "action");

            InitializeAdd(action, null, -1);
        }
        else
        {
            InitializeAddOrRemove(action, new object[] { changedItem }, -1);
        }
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
    {
        if ((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)
                && (action != NotifyCollectionChangedAction.Reset))
            throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action");

        if (action == NotifyCollectionChangedAction.Reset)
        {
            if (changedItem != null)
                throw new ArgumentException("ResetActionRequiresNullItem", "action");
            if (index != -1)
                throw new ArgumentException("ResetActionRequiresIndexMinus1", "action");

            InitializeAdd(action, null, -1);
        }
        else
        {
            InitializeAddOrRemove(action, new object[] { changedItem }, index);
        }
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
    {
        if ((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)
                && (action != NotifyCollectionChangedAction.Reset))
            throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action");

        if (action == NotifyCollectionChangedAction.Reset)
        {
            if (changedItems != null)
                throw new ArgumentException("ResetActionRequiresNullItem", "action");

            InitializeAdd(action, null, -1);
        }
        else
        {
            if (changedItems == null)
                throw new ArgumentNullException("changedItems");

            InitializeAddOrRemove(action, changedItems, -1);
        }
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
    {
        if ((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)
                && (action != NotifyCollectionChangedAction.Reset))
            throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor", "action");

        if (action == NotifyCollectionChangedAction.Reset)
        {
            if (changedItems != null)
                throw new ArgumentException("ResetActionRequiresNullItem", "action");
            if (startingIndex != -1)
                throw new ArgumentException("ResetActionRequiresIndexMinus1", "action");

            InitializeAdd(action, null, -1);
        }
        else
        {
            if (changedItems == null)
                throw new ArgumentNullException("changedItems");
            if (startingIndex < -1)
                throw new ArgumentException("IndexCannotBeNegative", "startingIndex");

            InitializeAddOrRemove(action, changedItems, startingIndex);
        }
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
    {
        if (action != NotifyCollectionChangedAction.Replace)
            throw new ArgumentException("NotifyCollectionChangedAction.Replace", "action");

        InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, -1, -1);
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
    {
        if (action != NotifyCollectionChangedAction.Replace)
            throw new ArgumentException("WrongActionForCtor, NotifyCollectionChangedAction.Replace", "action");

        InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, index);
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
    {
        if (action != NotifyCollectionChangedAction.Replace)
            throw new ArgumentException("WrongActionForCtor, NotifyCollectionChangedAction.Replace", "action");
        if (newItems == null)
            throw new ArgumentNullException("newItems");
        if (oldItems == null)
            throw new ArgumentNullException("oldItems");

        InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
    {
        if (action != NotifyCollectionChangedAction.Replace)
            throw new ArgumentException("WrongActionForCtor, NotifyCollectionChangedAction.Replace", "action");
        if (newItems == null)
            throw new ArgumentNullException("newItems");
        if (oldItems == null)
            throw new ArgumentNullException("oldItems");

        InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
    {
        if (action != NotifyCollectionChangedAction.Move)
            throw new ArgumentException("WrongActionForCtor, NotifyCollectionChangedAction.Move", "action");
        if (index < 0)
            throw new ArgumentException("IndexCannotBeNegative", "index");

        object[] changedItems = new object[] { changedItem };
        InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
    }

    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
    {
        if (action != NotifyCollectionChangedAction.Move)
            throw new ArgumentException("WrongActionForCtor, NotifyCollectionChangedAction.Move", "action");
        if (index < 0)
            throw new ArgumentException("IndexCannotBeNegative", "index");

        InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
    }

    private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
    {
        if (action == NotifyCollectionChangedAction.Add)
            InitializeAdd(action, changedItems, startingIndex);
        else if (action == NotifyCollectionChangedAction.Remove)
            InitializeRemove(action, changedItems, startingIndex);
        else
            throw new Exception(string.Format("Unsupported action: {0}", action.ToString()));
    }

    private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
    {
        Action = action;
        NewItems = (newItems == null) ? null : ArrayList.ReadOnly(newItems);
        NewStartingIndex = newStartingIndex;
    }

    private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
    {
        Action = action;
        OldItems = (oldItems == null) ? null : ArrayList.ReadOnly(oldItems);
        OldStartingIndex = oldStartingIndex;
    }

    private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
    {
        InitializeAdd(action, newItems, startingIndex);
        InitializeRemove(action, oldItems, oldStartingIndex);
    }
}

public enum NotifyCollectionChangedAction
{
    Add,
    Remove,
    Replace,
    Move,
    Reset,
}
#endif
